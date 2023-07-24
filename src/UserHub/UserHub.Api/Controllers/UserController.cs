using Microsoft.AspNetCore.Mvc;
using UserHub.Api.Contracts.Users;
using UserHub.Api.Data.Users;
using UserHub.Api.Domain;

namespace UserHub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserController> _logger;

    public UserController(
        ILogger<UserController> logger,
        IUserService userService,
        IUserRepository userRepository)
    {
        _logger = logger;
        _userService = userService;
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsersAsync()
    {
        var users = await _userService.GetAllUsers();
        return Ok(users);
    }

    [HttpGet("{Id}", Name = "GetUserByIdAsync")]
    public async Task<ActionResult<User>> GetUserByIdAsync(Guid Id)
    {
        var user = await _userService.GetUserById(Id);

        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<User>> CreateUserAsync([FromBody] User user) 
    {
        var result = await _userService.CreateUser(user);
        return CreatedAtRoute(nameof(GetUserByIdAsync), result);
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
        await _userRepository.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{Id}")]
    public async Task<ActionResult> DeleteUserAsync(Guid id)
    {
        var user = await _userService.GetUserById(id);

        if (user == null)
        {
            return NotFound();
        }

        _userService.RemoveUser(user);
        await _userRepository.SaveChangesAsync();
        return NoContent();
    }
}
