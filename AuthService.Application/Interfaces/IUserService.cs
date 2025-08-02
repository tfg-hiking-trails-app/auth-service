using AuthService.Application.DTOs;
using AuthService.Application.DTOs.Create;
using AuthService.Application.DTOs.Update;
using Common.Application.Interfaces;

namespace AuthService.Application.Interfaces;

public interface IUserService : IService<UserEntityDto, CreateUserEntityDto, UpdateUserEntityDto>
{
}