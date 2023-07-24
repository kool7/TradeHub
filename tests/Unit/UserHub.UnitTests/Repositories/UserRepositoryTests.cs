using AutoFixture;
using FluentAssertions;
using MentorsManagement.UnitTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Moq;
using UserHub.Api.Data;
using UserHub.Api.Data.Users;
using UserHub.Api.Domain;

namespace UserHub.UnitTests.Repositories
{
    public class UserRepositoryTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<UserHubDbContext> _userHubDbContext;
        private readonly UserRepositoryDb _userRepository;

        public UserRepositoryTests()
        {
            var dbContextOptions = new DbContextOptionsBuilder<UserHubDbContext>()
                .UseInMemoryDatabase("UserHubTesting")
                .Options;

            _fixture = new Fixture();
            _userHubDbContext = new Mock<UserHubDbContext>(dbContextOptions);
            _userRepository = new UserRepositoryDb(_userHubDbContext.Object);
        }

        [Fact]
        public async Task GetAll_Users_ReturnListOfUsers()
        {
            // Arrange
            var users = _fixture.CreateMany<User>(3).ToList();
            var mockDbSet = new Mock<DbSet<User>>();
            var queryableData = users.AsQueryable();

            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(queryableData.GetEnumerator());
            mockDbSet.As<IAsyncEnumerable<User>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new MockAsyncEnumerator<User>(queryableData.GetEnumerator()));

            _userHubDbContext.Setup(c => c.Users).Returns(mockDbSet.Object);

            // Act
            var result = await _userRepository.GetAll();

            // Assert
            result.Should().BeEquivalentTo(users);
        }

        [Fact]
        public async Task GetUser_IfUserExists_ReturnUser()
        {
            // Arrange
            var user = _fixture.Create<User>();
            var mockDbSet = new Mock<DbSet<User>>();
            _userHubDbContext.Setup(s => s.Set<User>()).Returns(mockDbSet.Object);

            // Act
            var result = await _userRepository.GetUser(user.Id);

            // Assert
            result.Should().Be(user);
            mockDbSet.Verify(m => m.Add(It.IsAny<User>()), Times.Once());
        }

        [Fact]
        public async Task CreateUser_AddUserToList_ReturnTrue()
        {
            // Arrange
            var user = _fixture.Create<User>();

            // Act
            var result = await _userRepository.CreateUser(user);

            // Assert
            result.Should().BeEquivalentTo(user);
        }

        [Fact]
        public async Task DeleteUser_RemovesUserFromList_ReturnTrue()
        {
            // Arrange
            var user = _fixture.Create<User>();
            await _userRepository.CreateUser(user);

            // Act
            var result = _userRepository.DeleteUser(user);

            // Assert
            _userHubDbContext.Verify(db => db.Remove(user), Times.Once);
        }
    }
}
