using AuthService.API.DTOs;
using AuthService.API.DTOs.Create;
using AuthService.API.DTOs.Filter;
using AuthService.API.Utils;
using AuthService.Application.Common.Pagination;
using AuthService.Application.DTOs;
using AuthService.Application.DTOs.Common;
using AuthService.Application.DTOs.Create;
using AuthService.Application.Interfaces;
using AuthService.Domain.Exceptions;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    
    public UserController(IUserService userService, IMapper mapper)
    {
        _userService = userService;
        _mapper = mapper;
    }
    
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<Page<UserDto>>> GetPaged(
        CancellationToken cancellationToken,
        [FromQuery] int pageNumber = Pagination.PAGE_NUMBER,
        [FromQuery] int pageSize = Pagination.PAGE_SIZE,
        [FromQuery] string sortField = Pagination.SORT_FIELD,
        [FromQuery] string sortDirection = Pagination.SORT_DIRECTION)
    {
        var users = await _userService.GetPaged(
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
    public ActionResult<UserDto> Post([FromBody] CreateUserDto userDto)
    {
        try
        {
            CreateUserEntityDto createUserEntityDto = _mapper.Map<CreateUserEntityDto>(userDto);
        
            Guid code = _userService.Create(createUserEntityDto);

            return Ok(code);
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
    
}