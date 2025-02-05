using Microsoft.EntityFrameworkCore;

namespace Authenticator_Application_Backend.Model
{
    public class AuthenticatorDbContext : DbContext
    {
        public AuthenticatorDbContext(DbContextOptions<AuthenticatorDbContext> options) : base(options)
        {

        }

        public DbSet<User> Users { get; set; }
    }
}
