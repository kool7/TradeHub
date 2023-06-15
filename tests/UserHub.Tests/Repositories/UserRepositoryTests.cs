using AutoFixture;
using FluentAssertions;
using UserHub.Api.Data.Users;
using UserHub.Api.Domain;

namespace UserHub.Tests.Repositories
{
    public class UserRepositoryTests
    {
        private readonly Fixture _fixture;
        private readonly UserRepository _userRepository;

        public UserRepositoryTests()
        {
            _fixture = new Fixture();
            _userRepository = new UserRepository();
        }

        [Fact]
        public async Task GetAll_Users_ReturnListOfUsers()
        {
            // Arrange
            var users = _fixture.CreateMany<User>(3);
            foreach (var user in users)
            {
                await _userRepository.CreateUser(user);
            }

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
            await _userRepository.CreateUser(user);
            
            // Act
            var result = await _userRepository.GetUser(user.Id);

            // Assert
            result.Should().Be(user);
            result.Should().BeOfType<User>();
        }

        [Fact]
        public async Task GetUser_IfUserDoesNotExists_ReturnNull()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var result = await _userRepository.GetUser(userId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateUser_AddUserToList_ReturnTrue()
        {
            // Arrange
            var user = _fixture.Create<User>();

            // Act
            var result = await _userRepository.CreateUser(user);

            // Assert
            result.Should().Be(true);
        }

        [Fact]
        public void DeleteUser_RemovesUserFromList_ReturnTrue()
        {
            // Arrange
            var user = _fixture.Create<User>();
            _userRepository.CreateUser(user);

            // Act
            var result = _userRepository.DeleteUser(user);

            // Assert
            result.Should().BeTrue();
        }
    }
}
