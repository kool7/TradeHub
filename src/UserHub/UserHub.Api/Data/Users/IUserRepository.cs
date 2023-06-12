using UserHub.Api.Modals;

namespace UserHub.Api.Data.Users
{
    public interface IUserRepository
    {
        void AddUser(User user);
    }
}
