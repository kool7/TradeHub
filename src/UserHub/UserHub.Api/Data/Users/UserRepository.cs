using Microsoft.EntityFrameworkCore;
using UserHub.Api.Domain;

namespace UserHub.Api.Data.Users
{
    public class UserRepository : IUserRepository
    {
        private static List<User> _users = new();
        private readonly UserHubDbContext _userHubDbContext;

        public UserRepository(UserHubDbContext userHubDbContext)
        {
            _userHubDbContext = userHubDbContext;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            //return await _userHubDbContext.Users.ToListAsync();
            return await Task.FromResult(_users);
        }

        public async Task<User> GetUser(Guid id)
        {
            return await Task.FromResult(_users.FirstOrDefault(user => user.Id == id)!);
        }

        public Task<bool> CreateUser(User user)
        {
            if (user != null)
            {
                user.Id = Guid.NewGuid();
                _users.Add(user);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public bool DeleteUser(User user)
        {
            return _users.Remove(user);
        }
    }
}
