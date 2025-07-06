using AuthService.Domain.Entities;

namespace AuthService.Domain.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByUserNameAsync(string userName);
}