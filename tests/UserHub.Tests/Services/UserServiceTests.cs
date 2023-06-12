using Moq;
using UserHub.Api.Data.Users;
using UserHub.Api.Domain;
using UserHub.Api.Services.Users;

namespace UserHub.Tests.Services;

public class UserTests
{
    [Fact]
    public void RegisterUser_ValidUser_ReturnsSuccess()
    {
        // Arrange
        var userRepository = new Mock<IUserRepository>();
        var userService = new UserService(userRepository.Object);
        var newUser = new User()
        {
            Id = Guid.NewGuid(),
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

    [Fact]
    public void Get_OnSuccess_InvokesUsersService()
    {
        // Arrange
        var mockUserService = new Mock<IUserRepository>();
        var sut = new UserService(mockUserService.Object);
        var newUser = new User()
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@example.com",
            Password = "password"
        };

        // Act
        sut.RegisterUser(newUser);

        // Assert
        mockUserService.Verify(services => services.AddUser(newUser), Times.Once());
    }


}
