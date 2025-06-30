using AuthService.Application.DTOs;
using AuthService.Application.DTOs.Create;
using AuthService.Application.DTOs.Update;
using AuthService.Domain.Entities;
using AutoMapper;

namespace AuthService.Infrastructure.Data.Configurations.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserEntityDto>().ReverseMap();
        CreateMap<User, CreateUserEntityDto>().ReverseMap();
        CreateMap<User, UpdateUserEntityDto>().ReverseMap();
    }
}