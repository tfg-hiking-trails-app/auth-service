using AuthService.Application.DTOs.Create;
using AuthService.Domain.Entities;
using AutoMapper;

namespace AuthService.Infrastructure.Data.Configurations.Mapping;

public class RefreshTokenEntityProfile : Profile
{
    public RefreshTokenEntityProfile()
    {
        CreateMap<CreateRefreshTokenEntityDto, RefreshToken>().ReverseMap();
    }
}