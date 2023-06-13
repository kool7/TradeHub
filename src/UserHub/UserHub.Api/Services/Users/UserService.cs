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

        public RegistrationResult RegisterUser(User newUser)
        {
            if (_userRepository.CreateUser(newUser))
            {
                return new RegistrationResult { Success = true };
            };

            return new RegistrationResult { Success = false };
        }
    }

    public class RegistrationResult
    {
        public bool Success { get; set; }
    }
}