using AuthService.Application.DTOs;
using AutoMapper;

namespace AuthService.API.DTOs.Mapping;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<RoleDto, RoleEntityDto>().ReverseMap();
    }
}