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

    public ActionResult<IList<User>> GetUsers()
    {
        var users = _userService.GetAllUsers();
        return Ok(users);
    }
}
