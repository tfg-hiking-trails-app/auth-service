using AuthService.API.DTOs.Create;
using AuthService.API.DTOs.Update;
using AuthService.Application.DTOs;
using AuthService.Application.DTOs.Create;
using AuthService.Application.DTOs.Update;
using AutoMapper;

namespace AuthService.API.DTOs.Mapping;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<RoleDto, RoleEntityDto>().ReverseMap();
        CreateMap<CreateRoleDto, CreateRoleEntityDto>().ReverseMap();
        CreateMap<UpdateRoleDto, UpdateRoleEntityDto>().ReverseMap();
    }
}