using AuthService.Application.DTOs;
using AuthService.Application.DTOs.Create;
using AuthService.Application.DTOs.Update;
using AuthService.Domain.Entities;
using AutoMapper;

namespace AuthService.Infrastructure.Data.Configurations.Mapping;

public class StatusEntityProfile : Profile
{
    public StatusEntityProfile()
    {
        CreateMap<StatusEntityDto, Status>().ReverseMap();
        CreateMap<CreateStatusEntityDto, Status>().ReverseMap();
        CreateMap<UpdateStatusEntityDto, Status>().ReverseMap();
    }
}