using AuthService.Application.DTOs;
using AuthService.Application.DTOs.Create;
using AuthService.Application.DTOs.Update;
using AuthService.Domain.Entities;
using AutoMapper;

namespace AuthService.Infrastructure.Data.Configurations.Mapping;

public class RoleEntityProfile : Profile
{
    public RoleEntityProfile()
    {
        CreateMap<RoleEntityDto, Role>().ReverseMap();
        CreateMap<CreateRoleEntityDto, Role>().ReverseMap();
        CreateMap<UpdateRoleEntityDto, Role>().ReverseMap();
    }
}