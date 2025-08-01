using AuthService.Domain.Entities;
using Common.Domain.Interfaces;

namespace AuthService.Domain.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByUserName(string userName);
}