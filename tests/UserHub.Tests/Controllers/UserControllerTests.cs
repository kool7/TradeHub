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
        [Fact]
        public async Task GetUsers_ReturnsListOfUsers()
        {
            // Arrange
            var mockUserService = new Mock<IUserService>();
            var mockLogger = new Mock<ILogger<UserController>>();
            var userList = new List<User>
            {
                new User() 
                {
                    Id = Guid.NewGuid(),
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "johndoe@example.com",
                    Password = "password"
                },
                new User()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "John",
                    LastName = "Smith",
                    Email = "johnsmith@example.com",
                    Password = "password"
                },
            };
            mockUserService.Setup(service => service.GetAllUsers())
                .Returns(userList);
            var controller = new UserController(mockLogger.Object,
                mockUserService.Object);

            // Act
            var result = controller.GetUsers();

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<IList<User>>(okObjectResult.Value);
            Assert.Equal(userList.Count, model.Count);
        }
    }
}
