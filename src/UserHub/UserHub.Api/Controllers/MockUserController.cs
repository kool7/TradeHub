using Microsoft.AspNetCore.Mvc;
using UserHub.Api.Contracts.Users;
using UserHub.Api.Data.Users;
using UserHub.Api.Domain;

namespace UserHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MockUserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<MockUserController> _logger;

    public MockUserController(
        ILogger<MockUserController> logger,
        IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    // Mock Repo
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetMockUsersAsync()
    {
        var users = await _userService.GetAllUsers();
        return Ok(users);
    }

    [HttpGet("{Id}", Name = "GetMockUserByIdAsync")]
    public async Task<ActionResult<User>> GetMockUserByIdAsync(Guid Id)
    {
        var user = await _userService.GetUserById(Id);

        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUserAsync(User user) 
    {
        var result = await _userService.CreateUser(user);
        return CreatedAtRoute(nameof(GetMockUserByIdAsync), new { Id = result.Id }, result);
    }

    [HttpPut("{Id}")]
    public async Task<ActionResult> UpdateUserAsync(Guid Id, UpdateUserDto updateUserDto)
    {
        var user = await _userService.GetUserById(Id);

        if (user == null)
        {
            return NotFound();
        }

        user.FirstName = updateUserDto.FirstName;
        user.LastName = updateUserDto.LastName;

        return NoContent();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteUserAsync(Guid id)
    {
        var user = await _userService.GetUserById(id);

        if (user == null)
        {
            return NotFound();
        }

        _userService.RemoveUser(user);
        return NoContent();
    }
}
