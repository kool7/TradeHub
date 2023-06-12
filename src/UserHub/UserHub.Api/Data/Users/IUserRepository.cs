using UserHub.Api.Domain;

namespace UserHub.Api.Data.Users
{
    public interface IUserRepository
    {
        void AddUser(User user);
    }
}
