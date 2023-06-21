using Microsoft.EntityFrameworkCore;
using UserHub.Api.Domain;

namespace UserHub.Api.Data
{
    public class UserHubDbContext : DbContext
    {
        public UserHubDbContext(DbContextOptions<UserHubDbContext> options) : base(options)
        {

        }

        public virtual DbSet<User> Users => Set<User>();
    }
}
