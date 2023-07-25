using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using UserHub.Api.Contracts.Users;
using UserHub.Api.Data;
using UserHub.Api.Domain;
using UserHub.IntegrationTests.Utility;

namespace UserHub.IntegrationTests.Controllers;

public class UserControllerIntegrationTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly Fixture _fixture;
    private readonly HttpClient _httpClient;
    private readonly WebApplicationFactory<Program> _appFactory;
    private readonly UserHubDbContext _dbContext;

    public UserControllerIntegrationTests()
    {
        _appFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll(typeof(DbContext));
                });
            });

        _httpClient = _appFactory.CreateClient();
        _fixture = new Fixture();

        using var scope = _appFactory.Services.CreateScope();
        _dbContext = scope.ServiceProvider.GetRequiredService<UserHubDbContext>();
        TestDbConfiguration.InitializeDbForTests(_dbContext);
    }

    [Fact]
    public async Task GetUsersAsync_Valid_EndpointReturnsSuccessStatus()
    {
        // Act
        var response = await _httpClient.GetAsync("https://localhost:44321/api/user");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?
            .ToString()
            .Should()
            .BeEquivalentTo("application/json; charset=utf-8");
        response.Content.ReadFromJsonAsync<IEnumerable<User>>().Result?.Count().Should().BeGreaterThanOrEqualTo(1); ;
    }

    [Fact]
    public async Task GetUserByIdAsync_ValidUserId_EndpointReturnsUser()
    {
        // Arrange
        var userId = new Guid("C9BD8871-F5B1-409F-53AE-08DB70BA51EF");

        // Act
        var response = await _httpClient.GetAsync($"https://localhost:44321/api/user/{userId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var user = await response.Content.ReadFromJsonAsync<User>();
        user?.Id.Should().Be(userId);
    }

    [Fact]
    public async Task GetUserByIdAsync_InValidUserId_EndpointReturnsNotFound()
    {
        // Arrange
        var userId = new Guid("C9BD8871-F5B1-409F-53AE-08DB70BA41EF");

        // Act
        var response = await _httpClient.GetAsync($"https://localhost:44321/api/user/{userId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task CreateUserAsync_CreatesUser_EndpointReturnsCreatedAtRouteResponse()
    {
        // Arrange
        var user = _fixture.Create<User>();
        HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(user),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _httpClient.PostAsync("https://localhost:44321/api/user", httpContent);

        // Assert
        var result = await response.Content.ReadFromJsonAsync<User>();

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Content.Headers.ContentType?
            .ToString()
            .Should()
            .BeEquivalentTo("application/json; charset=utf-8");
        result.Should().BeEquivalentTo(user);
    }

    [Fact]
    public async Task UpdateUserAsync_UpdatesUser_EndpointReturnsNoContent()
    {
        // Arrange
        var userId = new Guid("C9BD8871-F5B1-409F-53AE-08DB70BA51EF");
        var userToUpdate = _fixture.Create<UpdateUserDto>();
        HttpContent httpContentupdatedUser = new StringContent(JsonConvert.SerializeObject(userToUpdate),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _httpClient.PutAsync($"https://localhost:44321/api/user/{userId}", httpContentupdatedUser);

        // Assert
        var getUserResponse = await _httpClient.GetAsync($"https://localhost:44321/api/user/{userId}");
        var userUpdated = await getUserResponse.Content.ReadFromJsonAsync<User>();

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        userUpdated?.FirstName.Should().Be(userToUpdate.FirstName);
        userUpdated?.LastName.Should().Be(userToUpdate.LastName);
    }

    [Fact]
    public async Task DeleteUserAsync_RemovesUser_EndpointReturnsNoContent()
    {
        // Arrange
        var userId = new Guid("2AB641F6-97F5-403C-B8D8-08DB70C6DD75");

        // Act
        var response = await _httpClient.DeleteAsync($"https://localhost:44321/api/user/{userId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var getUserResponse = await _httpClient.GetAsync($"https://localhost:44321/api/user/{userId}");
        getUserResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}