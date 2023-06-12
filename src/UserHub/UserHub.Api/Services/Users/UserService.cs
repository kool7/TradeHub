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

        public RegistrationResult RegisterUser(User newUser)
        {
            _userRepository.AddUser(newUser);
            return new RegistrationResult { Success = true };
        }
    }

    public class RegistrationResult
    {
        public bool Success { get; set; }
    }
}