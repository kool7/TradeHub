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

    public ActionResult<List<User>> GetUsers()
    {
        var users = _userService.GetAllUsers();
        return Ok(users);
    }

    [HttpGet("{Id}", Name = "GetUserById")]
    public ActionResult<User> GetUserById(Guid Id)
    {
        var user = _userService.GetUserById(Id);

        if (user == null)
            return NotFound();

        return Ok(user);
    }
}
