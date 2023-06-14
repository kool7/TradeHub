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
        private readonly UserController _sutuserController;

        public UserControllerTests()
        {
            _fixture = new Fixture();
            _mockLogger = new Mock<ILogger<UserController>>();
            _mockUserService = new Mock<IUserService>();
            _sutuserController = new UserController(_mockLogger.Object, _mockUserService.Object);
        }

        [Fact]
        public async Task GetUsersAsync_ListOfUsers_ReturnsOkStatus()
        {
            // Arrange
            var expectedUsers = _fixture.CreateMany<User>();
            _mockUserService.Setup(service => service.GetAllUsers())
                .ReturnsAsync(expectedUsers);

            // Act
            var result = await _sutuserController.GetUsersAsync();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var users = Assert.IsAssignableFrom<IEnumerable<User>>(okObjectResult.Value);
            Assert.Equal(expectedUsers.Count(), users.Count());
            Assert.Equal(expectedUsers, users);
        }

        [Fact]
        public async Task GetUsersAsync_WhenNoUsersFound_ReturnsEmptyList()
        {
            // Arrange
            var expectedEmptyUsers = new List<User>();
            _mockUserService.Setup(service => service.GetAllUsers())
                .ReturnsAsync(expectedEmptyUsers);

            // Act
            var result = await _sutuserController.GetUsersAsync();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var users = Assert.IsAssignableFrom<IEnumerable<User>>(okObjectResult.Value);
            Assert.Empty(users);
        }

        [Fact]
        public async Task GetUserByIdAsync_ValidUserId_ReturnsUserOkStatus()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = _fixture.Create<User>();
            _mockUserService
                .Setup(service => service.GetUserById(userId))
                .ReturnsAsync(user);

            // Act
            var result = await _sutuserController.GetUserByIdAsync(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedUser = Assert.IsAssignableFrom<User>(okResult.Value);
            Assert.Equal(user.Id, returnedUser.Id);
        }

        [Fact]
        public async Task GetUserByIdAsync_InValidUserId_ReturnsNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = _fixture.Create<User>();
            _mockUserService
                .Setup(service => service.GetUserById(userId))
                .ReturnsAsync((User)null!);

            // Act
            var result = await _sutuserController.GetUserByIdAsync(userId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task CreateUserAsync_ReturnsCreatedAtRoute()
        {
            // Arrange
            var user = _fixture.Create<User>();
            _mockUserService
                .Setup(service => service.CreateUser(user))
                .ReturnsAsync(true);

            // Act
            var result = await _sutuserController.CreateUserAsync(user);

            // Assert
            Assert.IsType<CreatedAtRouteResult>(result.Result);

            var createdAtRouteResult = (CreatedAtRouteResult)result.Result;

            Assert.Equal("GetUserByIdAsync", createdAtRouteResult.RouteName);
            Assert.Equal(user.Id, createdAtRouteResult.RouteValues!["Id"]);
        }

        [Fact]
        public async Task UpdateUserAsync_ValidUserId_ReturnNoContent()
        {
            // Arrange
            var existingUser = _fixture.Create<User>();
            var userToUpdate = _fixture.Create<UpdateUserDto>();
            var userId = existingUser.Id;
            _mockUserService
                .Setup(service => service.GetUserById(existingUser.Id))
                .ReturnsAsync(existingUser);

            // Act
            var result = await _sutuserController.UpdateUserAsync(userId, userToUpdate);

            // Assert
            Assert.IsType<NoContentResult>(result);
            Assert.Equal(userToUpdate.FirstName, existingUser.FirstName);
            Assert.Equal(userToUpdate.LastName, existingUser.LastName);
        }

        [Fact]
        public async Task UpdateUserAsync_InValidUserId_ReturnNotFound()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var userToUpdate = _fixture.Create<UpdateUserDto>();

            // Act
            var result = await _sutuserController.UpdateUserAsync(userId, userToUpdate);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
