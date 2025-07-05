using AuthService.API.DTOs.Create;
using AuthService.API.DTOs.Update;
using AuthService.Application.DTOs;
using AuthService.Application.DTOs.Create;
using AuthService.Application.DTOs.Update;
using AutoMapper;

namespace AuthService.API.DTOs.Mapping;

public class StatusProfile : Profile
{
    public StatusProfile()
    {
        CreateMap<StatusDto, StatusEntityDto>().ReverseMap();
        CreateMap<CreateStatusDto, CreateStatusEntityDto>().ReverseMap();
        CreateMap<UpdateStatusDto, UpdateStatusEntityDto>().ReverseMap();
    }
}