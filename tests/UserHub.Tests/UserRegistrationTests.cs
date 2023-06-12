using Moq;
using UserHub.Api.Data.Users;
using UserHub.Api.Modals;
using UserHub.Api.Services.Users;

namespace UserHub.Tests;

public class UserRegistrationTests
{
    [Fact]
    public void RegisterUser_ValidUser_ReturnsSuccess()
    {
        // Arrange
        var userRepository = new Mock<IUserRepository>();
        var userService = new UserService(userRepository.Object);
        var newUser = new User()
        {
            Id = 1,
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@example.com",
            Password = "password"
        };

        // Act
        var result = userService.RegisterUser(newUser);

        // Assert
        Assert.True(result.Success);
    }
}
