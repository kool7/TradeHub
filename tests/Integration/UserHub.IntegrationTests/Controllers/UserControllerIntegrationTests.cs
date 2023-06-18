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
        var dbContext = scope.ServiceProvider.GetRequiredService<UserHubDbContext>();
        TestDbConfiguration.InitializeDbForTests(dbContext);
    }

    [Fact]
    public async Task GetUsersAsync_EmptyListsOfUsers_EndpointReturnsSuccessStatus()
    {
        // Act
        var response = await _httpClient.GetAsync("https://localhost:44321/api/user");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?
            .ToString()
            .Should()
            .BeEquivalentTo("application/json; charset=utf-8");
        response.Content.ReadFromJsonAsync<IEnumerable<User>>().Should().NotBeNull();
    }

    [Fact]
    public async Task GetUsersAsync_ListsOfUsers_EndpointReturnsSuccessStatus()
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
        var user = _fixture.Create<User>();
        HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(user),
            Encoding.UTF8,
            "application/json");
        var newUser = await _httpClient.PostAsync("https://localhost:44321/api/user", httpContent);
        var newUserwithId = await newUser.Content.ReadFromJsonAsync<User>();
        var Id = newUserwithId?.Id;

        // Act
        var response = await _httpClient.GetAsync($"https://localhost:44321/api/user/{Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.ReadFromJsonAsync<User>().Should().NotBeNull();
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
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        response.Content.Headers.ContentType?
            .ToString()
            .Should()
            .BeEquivalentTo("application/json; charset=utf-8");
        response.Content.ReadFromJsonAsync<User>().Should().NotBeNull();
    }

    [Fact]
    public async Task UpdateUserAsync_UpdatesUser_EndpointReturnsNoContent()
    {
        // Arrange
        var user = _fixture.Create<User>();
        HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(user),
            Encoding.UTF8,
            "application/json");
        var userToUpdate = await _httpClient.PostAsync("https://localhost:44321/api/user", httpContent);
        var userToUpdateId = await userToUpdate.Content.ReadFromJsonAsync<User>();
        var Id = userToUpdateId?.Id;

        var updatedUser = _fixture.Create<UpdateUserDto>();
        HttpContent httpContentupdatedUser = new StringContent(JsonConvert.SerializeObject(updatedUser),
            Encoding.UTF8,
            "application/json");

        // Act
        var response = await _httpClient.PutAsync($"https://localhost:44321/api/user/{Id}", httpContentupdatedUser);

        // Assert
        var getUserResponse = await _httpClient.GetAsync($"https://localhost:44321/api/user/{Id}");
        getUserResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var userUpdated = await getUserResponse.Content.ReadFromJsonAsync<User>();
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        userUpdated?.FirstName.Should().Be(user.FirstName);
        userUpdated?.LastName.Should().Be(user.LastName);
    }
}