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

        public async Task<User?> CreateUser(User user)
        {
            if (user != null)
            {
                user.Id = Guid.NewGuid();
                _users.Add(user);
            }

            return await Task.FromResult(user);
        }

        public async Task DeleteUser(User user)
        {
            await Task.FromResult(_users.Remove(user));
        }

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
