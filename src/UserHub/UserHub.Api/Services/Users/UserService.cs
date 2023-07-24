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

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _userRepository.GetAll();
        }

        public async Task<User> CreateUser(User newUser)
        {
            return await _userRepository.CreateUser(newUser);
        }

        public async Task<User> GetUserById(Guid Id)
        {
            return await _userRepository.GetUser(Id);
        }

        public async Task RemoveUser(User user)
        {
            await _userRepository.DeleteUser(user);
        }
    }

    public class RegistrationResult
    {
        public bool Success { get; set; }
    }
}