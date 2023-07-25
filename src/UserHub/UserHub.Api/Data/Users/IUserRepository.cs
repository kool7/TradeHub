using UserHub.Api.Contracts.Users;
using UserHub.Api.Domain;

namespace UserHub.Api.Data.Users
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> GetUser(Guid id);
        Task<User> CreateUser(User user);
        Task DeleteUser(User user);
        Task SaveChangesAsync();
    }
}
