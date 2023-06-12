using FluentAssertions;
using UserHub.Api.Domain;

namespace UserHub.Tests.Entities;

public class UserTests
{
    [Fact]
    public void CreateUser_WithValidProperties()
    {
        // Arrange
        var id = Guid.NewGuid();
        var firstName = "John";
        var lastName = "Doe";
        var email = "john.doe@example.com";
        var password = "P@ssw0rd";

        // Act
        var user = new User()
        {
            Id = id,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            Password = password
        };

        // Assert
        user.Id.Should().Be(id);
        user.FirstName.Should().Be(firstName);
        user.LastName.Should().Be(lastName);
        user.Email.Should().Be(email);
        user.Password.Should().Be(password);
    }

}
