using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UserHub.Api.Data;
using UserHub.Api.Domain;

namespace UserHub.IntegrationTests.Utility
{
    public static class TestDbConfiguration
    {
        public static IConfiguration GetConfiguration() =>
            new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.testing.json", true, true)
                .Build();

        public static UserHubDbContext GetContext(IConfiguration configuration)
        {
            var optionsBuilder = new DbContextOptionsBuilder<UserHubDbContext>();
            var connectionString = configuration.GetConnectionString("SoftwareTesting");
            optionsBuilder.UseSqlServer("server=CESIT-LAP-0251;Initial Catalog=UserHubTesting;User ID=CESLTD\\kuldeepsingh.chouhan;Trusted_Connection=true");
            return new UserHubDbContext(optionsBuilder.Options);
        }

        public static List<User> GetSeedingUsers()
        {
            return new List<User>()
            {
                new User() { Id = new Guid("C9BD8871-F5B1-409F-53AE-08DB70BA51EF"), FirstName = "Kuldeep", LastName = "Singh Chouhan", Email = "kschouhan714@gmail.com", Password = "Pa$$w@rd"},
                new User() { FirstName = "Chetan", LastName = "Singh Chouhan", Email = "chetan24@gmail.com", Password = "Pa$$w@rd"},
                new User() { Id = new Guid("2AB641F6-97F5-403C-B8D8-08DB70C6DD75"), FirstName = "Piyush", LastName = "Thakur", Email = "piyush@gmail.com", Password = "Pa$$w@rd"}
            };
        }

        public static void InitializeDbForTests(UserHubDbContext db)
        {
            db.Users.RemoveRange(db.Users);
            var users = GetSeedingUsers();
            db.Users.AddRange(users);
            db.SaveChanges();
        }
    }
}