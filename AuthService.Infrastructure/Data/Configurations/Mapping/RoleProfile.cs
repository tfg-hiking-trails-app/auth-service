using AuthService.Application.DTOs;
using AuthService.Application.DTOs.Create;
using AuthService.Application.DTOs.Update;
using AuthService.Domain.Entities;
using AutoMapper;

namespace AuthService.Infrastructure.Data.Configurations.Mapping;

public class RoleProfile : Profile
{
    public RoleProfile()
    {
        CreateMap<Role, RoleEntityDto>().ReverseMap();
        CreateMap<Role, CreateRoleEntityDto>().ReverseMap();
        CreateMap<Role, UpdateRoleEntityDto>().ReverseMap();
    }
}