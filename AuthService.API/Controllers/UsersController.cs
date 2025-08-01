using AuthService.API.DTOs;
using AuthService.API.DTOs.Create;
using AuthService.API.DTOs.Update;
using AuthService.Application.DTOs;
using AuthService.Application.DTOs.Create;
using AuthService.Application.DTOs.Update;
using AuthService.Application.Interfaces;
using AutoMapper;
using Common.API.DTOs.Filter;
using Common.API.Utils;
using Common.Application.DTOs.Filter;
using Common.Application.Pagination;
using Common.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    
    public UsersController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Page<UserDto>>> GetPaged(
        CancellationToken cancellationToken,
        [FromQuery] int pageNumber = Pagination.PageNumber,
        [FromQuery] int pageSize = Pagination.PageSize,
        [FromQuery] string sortField = Pagination.SortField,
        [FromQuery] string sortDirection = Pagination.SortDirection)
    {
        var users = await _userService.GetPagedAsync(
            _mapper.Map<FilterEntityDto>(new FilterDto(pageNumber, pageSize, sortField, sortDirection)), 
                cancellationToken
        );
        
        return Ok(
            _mapper.Map<Page<UserDto>>(users)
        );
    }

    [HttpGet("{code}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDto>> GetByCode(string code)
    {
        try
        {
            UserEntityDto userEntityDto = await _userService.GetByCodeAsync(Guid.Parse(code));

            return Ok(_mapper.Map<UserDto>(userEntityDto));
        }
        catch (NotFoundEntityException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDto>> Post([FromBody] CreateUserDto userDto)
    {
        try
        {
            CreateUserEntityDto createUserEntityDto = _mapper.Map<CreateUserEntityDto>(userDto);
        
            await _userService.CreateAsync(createUserEntityDto);

            return Created();
        }
        catch (EntityAlreadyExistsException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDto>> Patch([FromBody] UpdateUserDto userDto)
    {
        try
        {
            if (userDto.Code is null)
                return BadRequest("Code is null");

            UpdateUserEntityDto updateUserEntityDto = _mapper.Map<UpdateUserEntityDto>(userDto);

            Guid code = await _userService.UpdateAsync(updateUserEntityDto);

            return Ok(code);
        }
        catch (NotFoundEntityException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Delete([FromBody] string code)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(code))
                return BadRequest("Code is null or empty");
            
            if (!Guid.TryParse(code, out Guid userCode))
                return BadRequest("Code must be Guid format");
            
            await _userService.DeleteAsync(userCode);
            
            return NoContent();
        }
        catch (NotFoundEntityException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
}