using Common.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Infrastructure.Data
{
    public class AuthServiceDbContext : AbstractDbContext
    {
        public AuthServiceDbContext(DbContextOptions<AuthServiceDbContext> options)
            : base(options)
        {
        }
    }
}
