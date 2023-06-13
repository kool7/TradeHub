using UserHub.Api.Domain;

namespace UserHub.Api.Data.Users
{
    public interface IUserRepository
    {
        bool CreateUser(User user);
        List<User> GetAll();
    }
}
