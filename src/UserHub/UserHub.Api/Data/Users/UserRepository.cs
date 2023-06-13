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

        public bool DeleteUser(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<User> GetAll()
        {
            return _users;
        }

        public User GetUser(Guid id)
        {
            return _users.FirstOrDefault(user => user.Id == id)!;
        }

        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }

        public bool UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
