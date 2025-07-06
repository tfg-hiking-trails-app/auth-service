using AuthService.Application.DTOs;
using AutoMapper;

namespace AuthService.API.DTOs.Mapping;

public class AuthenticationProfile : Profile
{
    public AuthenticationProfile()
    {
        CreateMap<AuthenticationDto, AuthenticationEntityDto>().ReverseMap();
    }
}