using UserHub.Api.Contracts.Users;
using UserHub.Api.Data.Users;
using UserHub.Api.Domain;

namespace UserHub.Api.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<User> GetAllUsers()
        {
            return _userRepository.GetAll();
        }

        public User CreateUser(User newUser)
        {
            if (_userRepository.CreateUser(newUser))
            {
                return newUser;
            };

            return newUser;
        }

        public User GetUserById(Guid Id)
        {
            var user = _userRepository.GetUser(Id);
            return user;
        }
    }

    public class RegistrationResult
    {
        public bool Success { get; set; }
    }
}