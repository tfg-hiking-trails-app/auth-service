using AuthService.Application.DTOs;
using AuthService.Application.DTOs.Create;
using AuthService.Application.DTOs.Update;
using AuthService.Domain.Entities;
using AutoMapper;

namespace AuthService.Infrastructure.Data.Configurations.Mapping;

public class UserEntityProfile : Profile
{
    public UserEntityProfile()
    {
        CreateMap<UserEntityDto, User>().ReverseMap();
        CreateMap<CreateUserEntityDto, User>().ReverseMap();
        CreateMap<UpdateUserEntityDto, User>().ReverseMap();
    }
}