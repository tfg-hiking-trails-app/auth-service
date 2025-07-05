using AuthService.API.DTOs;
using AuthService.API.DTOs.Create;
using AuthService.Application.DTOs;
using AuthService.Application.DTOs.Create;
using AuthService.Application.Interfaces;
using AuthService.Domain.Exceptions;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers;

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
    public ActionResult<IEnumerable<UserDto>> GetAll()
    {
        return Ok(
            _mapper.Map<IEnumerable<UserDto>>(_userService.GetAll())
        );
    }

    [HttpGet("async")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<UserDto>> GetAllAsync()
    {
        return Ok(
            _mapper.Map<IEnumerable<UserDto>>(await _userService.GetAllAsync())
        );
    }
    
    [HttpGet("{code}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<UserDto> GetByCode(string code)
    {
        try
        {
            UserEntityDto userEntityDto = _userService.GetByCode(Guid.Parse(code));

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

    [HttpGet("async/{code}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserDto>> GetByCodeAsync(string code)
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