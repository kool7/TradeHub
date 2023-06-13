using Moq;
using UserHub.Api.Data.Users;
using UserHub.Api.Domain;
using UserHub.Api.Services.Users;

namespace UserHub.Tests.Services;

public class UserTests
{
    [Fact]
    public void RegisterUser_UserCreated_ReturnsTrue()
    {
        // Arrange
        var mockUserRepository = new Mock<IUserRepository>();
        mockUserRepository
            .Setup(repo => repo.CreateUser(It.IsAny<User>()))
            .Returns(true);
        var sutUserService = new UserService(mockUserRepository.Object);

        var newUser = new User()
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@example.com",
            Password = "password"
        };

        // Act
        var result = sutUserService.RegisterUser(newUser);

        // Assert
        Assert.True(result.Success);
    }

    [Fact]
    public void RegisterUser_OnSuccess_InvokesUserService()
    {
        // Arrange
        var mockUserRepository = new Mock<IUserRepository>();
        var sutUserService = new UserService(mockUserRepository.Object);
        var newUser = new User()
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "johndoe@example.com",
            Password = "password"
        };

        // Act
        sutUserService.RegisterUser(newUser);

        // Assert
        mockUserRepository.Verify(services => services.CreateUser(newUser), Times.Once());
    }


}
