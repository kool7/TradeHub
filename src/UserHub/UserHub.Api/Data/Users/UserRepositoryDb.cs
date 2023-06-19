using Microsoft.EntityFrameworkCore;
using UserHub.Api.Domain;

namespace UserHub.Api.Data.Users
{
    public class UserRepositoryDb : IUserRepository
    {
        private readonly UserHubDbContext _userHubDbContext;

        public UserRepositoryDb(UserHubDbContext userHubDbContext)
        {
            _userHubDbContext = userHubDbContext;
        }

        public async Task<bool> CreateUser(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            await _userHubDbContext.AddAsync(user);
            await SaveChangesAsync();
            return true;
        }

        public bool DeleteUser(User user)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            _userHubDbContext.Remove(user);
            return true;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _userHubDbContext.Users.ToListAsync();
        }

        public async Task<User> GetUser(Guid id)
        {
            return await _userHubDbContext.Users.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _userHubDbContext.SaveChangesAsync();
        }
    }
}
