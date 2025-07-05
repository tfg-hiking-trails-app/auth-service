using AuthService.Application.DTOs;
using AutoMapper;

namespace AuthService.API.DTOs.Mapping;

public class StatusProfile : Profile
{
    public StatusProfile()
    {
        CreateMap<StatusDto, StatusEntityDto>().ReverseMap();
    }
}