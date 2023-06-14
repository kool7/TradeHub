using AutoFixture;
using Moq;
using UserHub.Api.Data.Users;
using UserHub.Api.Domain;
using UserHub.Api.Services.Users;

namespace UserHub.Tests.Services;

public class UserServiceTests
{
    private readonly Fixture _fixture;
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly UserService _sutUserService;

    public UserServiceTests()
    {
        _fixture = new Fixture();
        _mockUserRepository = new Mock<IUserRepository>();
        _sutUserService = new UserService(_mockUserRepository.Object);
    }

    [Fact]
    public async Task CreateUser_UserCreated_ReturnsTrue()
    {
        // Arrange
        _mockUserRepository
            .Setup(repo => repo.CreateUser(It.IsAny<User>()))
            .ReturnsAsync(true);

        var newUser = _fixture.Create<User>();

        // Act
        var result = await _sutUserService.CreateUser(newUser);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CreateUser_OnSuccess_InvokesUserService()
    {
        // Arrange
        var newUser = _fixture.Create<User>();

        // Act
        await _sutUserService.CreateUser(newUser);

        // Assert
        _mockUserRepository.Verify(services => services.CreateUser(newUser), Times.Once());
    }
}
