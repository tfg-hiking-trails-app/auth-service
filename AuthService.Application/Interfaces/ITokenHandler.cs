using AuthService.Domain.Entities;

namespace AuthService.Application.Interfaces;

public interface ITokenHandler
{
    string GenerateAccessToken(User user);
}