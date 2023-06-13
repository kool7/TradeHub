using UserHub.Api.Domain;

namespace UserHub.Api.Data.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users = new();

        public bool CreateUser(User user)
        {
            if (user != null)
            {
                _users.Add(user);
                return true;
            }

            return false;
        }

        public List<User> GetAll()
        {
            return _users;
        }
    }
}
