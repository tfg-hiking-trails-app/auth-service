using AuthService.API.DTOs;
using AuthService.API.DTOs.Create;
using AuthService.API.DTOs.Update;
using AuthService.Application.DTOs;
using AuthService.Application.DTOs.Create;
using AuthService.Application.DTOs.Update;
using AuthService.Application.Interfaces;
using AutoMapper;
using Common.API.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class UsersController : AbstractController<UserDto, CreateUserDto, 
    UpdateUserDto, UserEntityDto, CreateUserEntityDto, UpdateUserEntityDto>
{
    public UsersController(
        IUserService userService, 
        IMapper mapper) : base(userService, mapper)
    {
    }
    
}