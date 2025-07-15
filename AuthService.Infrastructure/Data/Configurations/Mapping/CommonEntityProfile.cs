using AuthService.Application.DTOs.Common;
using AuthService.Domain.Common;
using AutoMapper;

namespace AuthService.Infrastructure.Data.Configurations.Mapping;

public class CommonEntityProfile : Profile
{
    public CommonEntityProfile()
    {
        CreateMap<FilterEntityDto, FilterData>().ReverseMap();
    }
}