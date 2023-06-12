using UserHub.Api.Domain;
using UserHub.Api.Services.Users;

namespace UserHub.Api.Contracts.Users
{
    public interface IUserService
    {
        RegistrationResult RegisterUser(User newUser);
    }
}
