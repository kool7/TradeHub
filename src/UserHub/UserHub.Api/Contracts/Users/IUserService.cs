using UserHub.Api.Modals;
using UserHub.Api.Services.Users;

namespace UserHub.Api.Contracts.Users
{
    public interface IUserService
    {
        RegistrationResult RegisterUser(User newUser);
    }
}
