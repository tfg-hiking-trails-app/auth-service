using AuthService.Domain.Entities;

namespace AuthService.Domain.Interfaces;

public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
    Task<RefreshToken?> FindByRefreshToken(string token);
}