using AuthService.API.DTOs.Filter;
using AuthService.Application.Common.Pagination;
using AuthService.Application.DTOs.Common;
using AuthService.Infrastructure.Converters;
using AutoMapper;

namespace AuthService.API.DTOs.Mapping;

public class CommonProfile : Profile
{
    public CommonProfile()
    {
        CreateMap<FilterDto, FilterEntityDto>().ReverseMap();
        CreateMap(typeof(Page<>), typeof(Page<>)).ConvertUsing(typeof(PageConverter<,>));
    }
}