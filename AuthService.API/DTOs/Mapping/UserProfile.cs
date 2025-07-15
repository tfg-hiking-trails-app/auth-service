using AuthService.API.DTOs.Create;
using AuthService.API.DTOs.Update;
using AuthService.Application.DTOs;
using AuthService.Application.DTOs.Create;
using AuthService.Application.DTOs.Update;
using AutoMapper;

namespace AuthService.API.DTOs.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserDto, UserEntityDto>().ReverseMap();
        CreateMap<CreateUserDto, CreateUserEntityDto>().ReverseMap();
        CreateMap<UpdateUserDto, UpdateUserEntityDto>().ReverseMap();
    }
}