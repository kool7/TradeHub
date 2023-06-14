using UserHub.Api.Domain;

namespace UserHub.Api.Data.Users
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> GetUser(Guid id);
        Task<bool> CreateUser(User user);
        bool DeleteUser(Guid id);
    }
}
