using UserHub.Api.Domain;

namespace UserHub.Api.Data.Users
{
    public interface IUserRepository
    {
        bool CreateUser(User user);
        User GetUser(Guid id);
        bool UpdateUser(User user);
        bool DeleteUser(Guid id);
        List<User> GetAll();
        bool SaveChanges();
    }
}
