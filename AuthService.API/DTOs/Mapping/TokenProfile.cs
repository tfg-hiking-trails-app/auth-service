using AuthService.Application.DTOs;
using AutoMapper;

namespace AuthService.API.DTOs.Mapping;

public class TokenProfile : Profile
{
    public TokenProfile()
    {
        CreateMap<TokenResponseDto, TokenResponseEntityDto>().ReverseMap();
    }
}