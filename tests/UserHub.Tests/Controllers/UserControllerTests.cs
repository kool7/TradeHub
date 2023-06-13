using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UserHub.Api.Contracts.Users;
using UserHub.Api.Controllers;
using UserHub.Api.Domain;

namespace UserHub.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<ILogger<UserController>> _mockLogger;
        private readonly Mock<IUserService> _mockUserService;
        private readonly UserController _userController;

        public UserControllerTests()
        {
            _fixture = new Fixture();
            _mockLogger = new Mock<ILogger<UserController>>();
            _mockUserService = new Mock<IUserService>();
            _userController = new UserController(_mockLogger.Object, _mockUserService.Object);
        }

        [Fact]
        public async Task GetUsers_ListOfUsers_ReturnsOkStatus()
        {
            // Arrange
            var expectedUsers = _fixture.Create<List<User>>();
            _mockUserService.Setup(service => service.GetAllUsers())
                .Returns(expectedUsers);

            // Act
            var result = _userController.GetUsers();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var users = Assert.IsAssignableFrom<List<User>>(okObjectResult.Value);
            Assert.Equal(expectedUsers.Count, users.Count);
            Assert.Equal(expectedUsers, users);
        }

        [Fact]
        public async Task GetUsers_WhenNoUsersFound_ReturnsEmptyList()
        {
            // Arrange
            var emptyUsers = new List<User>();
            _mockUserService.Setup(service => service.GetAllUsers())
                .Returns(emptyUsers);

            // Act
            var result = _userController.GetUsers();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var users = Assert.IsType<List<User>>(okObjectResult.Value);
            Assert.Empty(users);
        }

        [Fact]
        public async Task GetUserById_ExistingUserId_ReturnsUserOkStatus()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = _fixture.Create<User>();
            _mockUserService
                .Setup(service => service.GetUserById(userId))
                .Returns(user);

            // Act
            var result = _userController.GetUserById(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUser = Assert.IsAssignableFrom<User>(okResult.Value);
            Assert.Equal(user.Id, returnedUser.Id);
        }

    }
}
