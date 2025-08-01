using AuthService.Domain.Entities;
using Common.Domain.Interfaces;

namespace AuthService.Domain.Interfaces;

public interface IRefreshTokenRepository : IRepository<RefreshToken>
{
    Task<RefreshToken?> FindByRefreshTokenAsync(string token);
    
    Task<IEnumerable<RefreshToken>> GetAllValidRefreshTokensByUserAsync(Guid userCode);
}