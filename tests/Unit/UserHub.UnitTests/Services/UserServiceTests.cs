using AutoFixture;
using FluentAssertions;
using Moq;
using UserHub.Api.Data.Users;
using UserHub.Api.Domain;
using UserHub.Api.Services.Users;

namespace UserHub.UnitTests.Services;

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
    public async Task GetAllUsers_IsNotEmpty_ReturnsIEnumerableOfUser()
    {
        // Arrange
        var users = _fixture.CreateMany<User>();
        _mockUserRepository
            .Setup(repo => repo.GetAll())
            .ReturnsAsync(users);

        // Act
        var result = await _sutUserService.GetAllUsers();

        // Assert
        result.Should().BeEquivalentTo(users);
    }

    [Fact]
    public async Task GetAllUsers_IsEmpty_ReturnsEmptyListOfUsers()
    {
        // Arrange
        var emptUsersList = new List<User>();
        _mockUserRepository
            .Setup(repo => repo.GetAll())
            .ReturnsAsync(emptUsersList);

        // Act
        var result = await _sutUserService.GetAllUsers();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetUserById_ValidUserId_ReturnsUser()
    {
        // Arrange
        var user = _fixture.Create<User>();
        var userId = user.Id;
        _mockUserRepository
            .Setup(repo => repo.GetUser(userId))
            .ReturnsAsync(user);

        // Act
        var result = await _sutUserService.GetUserById(userId);

        // Assert
        result.Should().BeEquivalentTo(user);
        result.Should().BeOfType<User>();
        result.Id.Should().Be(userId);
    }

    [Fact]
    public async Task GetUserById_InValidUserId_ReturnsUser()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _mockUserRepository
            .Setup(repo => repo.GetUser(userId))
            .ReturnsAsync((User)null!);

        // Act
        var result = await _sutUserService.GetUserById(userId);

        // Assert
        result.Should().BeNull();
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
        result.Should().BeTrue();
    }

    [Fact]
    public async Task CreateUser_UserNotCreated_ReturnsFalse()
    {
        // Arrange
        _mockUserRepository
            .Setup(repo => repo.CreateUser(It.IsAny<User>()))
            .ReturnsAsync(false);

        var newUser = (User)null!;

        // Act
        var result = await _sutUserService.CreateUser(newUser);

        // Assert
        result.Should().BeFalse();
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

    [Fact]
    public void RemoveUser_IfUserExists_ReturnsTrue()
    {
        // Arrange
        var newUser = _fixture.Create<User>();
        _mockUserRepository
                .Setup(service => service.GetUser(newUser.Id))
                .ReturnsAsync(newUser);

        // Act
        var result = _sutUserService.RemoveUser(newUser);

        // Assert
        _mockUserRepository.Verify(services => services.DeleteUser(newUser), Times.Once());
    }
}
