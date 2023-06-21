using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using UserHub.Api.Contracts.Users;
using UserHub.Api.Controllers;
using UserHub.Api.Domain;

namespace UserHub.UnitTests.Controllers
{
    public class UserControllerTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<ILogger<MockUserController>> _mockLogger;
        private readonly Mock<IUserService> _mockUserService;
        private readonly MockUserController _sutuserController;

        public UserControllerTests()
        {
            _fixture = new Fixture();
            _mockLogger = new Mock<ILogger<MockUserController>>();
            _mockUserService = new Mock<IUserService>();
            _sutuserController = new MockUserController(_mockLogger.Object, _mockUserService.Object);
        }

        [Fact]
        public async Task GetMockUsersAsync_ListOfUsers_ReturnsOkStatus()
        {
            // Arrange
            var expectedUsers = _fixture.CreateMany<User>();
            _mockUserService.Setup(service => service.GetAllUsers())
                .ReturnsAsync(expectedUsers);

            // Act
            var result = await _sutuserController.GetMockUsersAsync();

            // Assert
            result.Should().NotBeNull();
            result.Value?.Count().Should().Be(expectedUsers.Count());
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetMockUsersAsync_WhenNoUsersFound_ReturnsEmptyList()
        {
            // Arrange
            var expectedEmptyUsers = new List<User>();
            _mockUserService.Setup(service => service.GetAllUsers())
                .ReturnsAsync(expectedEmptyUsers);

            // Act
            var result = await _sutuserController.GetMockUsersAsync();

            // Assert
            result.Value.Should().BeNullOrEmpty();
            result.Value?.Count().Should().Be(0);
            result.Result.Should().BeOfType<OkObjectResult>();
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
            var result = await _sutuserController.GetMockUserByIdAsync(userId);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            result.Value?.Id.Should().Be(user.Id);
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
            var result = await _sutuserController.GetMockUserByIdAsync(userId);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
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
            result.Result.Should().BeOfType<CreatedAtRouteResult>();
            result.Value?.Should().BeEquivalentTo(user);

            var createdAtRouteResult = (CreatedAtRouteResult)result.Result!;
            createdAtRouteResult?.StatusCode.Should().Be(201);
            createdAtRouteResult?.RouteName.Should().BeEquivalentTo("GetMockUserByIdAsync");
            createdAtRouteResult?.RouteValues!["Id"].Should().BeEquivalentTo(user.Id);
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
            result.Should().BeOfType<NoContentResult>();
            userToUpdate.FirstName.Should().BeEquivalentTo(existingUser.FirstName);
            userToUpdate.LastName.Should().BeEquivalentTo(existingUser.LastName);
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
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteUserAsync_ValidUserId_RemovesUser()
        {
            // Arrange
            var userToDelete = _fixture.Create<User>();
            _mockUserService
                .Setup(service => service.GetUserById(userToDelete.Id))
                .ReturnsAsync(userToDelete);

            // Act
            var result = await _sutuserController.DeleteUserAsync(userToDelete.Id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _mockUserService.Verify(service => service.RemoveUser(userToDelete), Times.Once);
        }
    }
}
