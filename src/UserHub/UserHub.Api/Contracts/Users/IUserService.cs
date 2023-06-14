using UserHub.Api.Domain;
using UserHub.Api.Services.Users;

namespace UserHub.Api.Contracts.Users
{
    public interface IUserService
    {
        Task<bool> CreateUser(User newUser);
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUserById(Guid id);
    }
}
