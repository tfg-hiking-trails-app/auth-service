using AuthService.Application.DTOs;
using AuthService.Application.DTOs.Create;
using AuthService.Application.DTOs.Update;
using AuthService.Domain.Entities;
using AutoMapper;

namespace AuthService.Infrastructure.Data.Configurations.Mapping;

public class StatusProfile : Profile
{
    public StatusProfile()
    {
        CreateMap<Status, StatusEntityDto>().ReverseMap();
        CreateMap<Status, CreateStatusEntityDto>().ReverseMap();
        CreateMap<Status, UpdateStatusEntityDto>().ReverseMap();
    }
}