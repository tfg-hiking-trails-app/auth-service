using AuthService.Application.DTOs;
using AutoMapper;

namespace AuthService.API.DTOs.Mapping;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserDto, UserEntityDto>().ReverseMap();
    }
}