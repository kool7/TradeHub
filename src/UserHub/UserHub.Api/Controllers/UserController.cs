using Microsoft.AspNetCore.Mvc;
using UserHub.Api.Contracts.Users;
using UserHub.Api.Domain;

namespace UserHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(
        ILogger<UserController> logger,
        IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsersAsync()
    {
        var users = await _userService.GetAllUsers();
        return Ok(users);
    }

    [HttpGet("{Id}", Name = "GetUserById")]
    public async Task<ActionResult<User>> GetUserByIdAsync(Guid Id)
    {
        var user = await _userService.GetUserById(Id);

        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUserAsync(User user) 
    {
        await _userService.CreateUser(user);
        return CreatedAtRoute(nameof(GetUserByIdAsync), new { Id = user.Id }, user);
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
