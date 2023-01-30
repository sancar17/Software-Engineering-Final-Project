using Backend.Models;
using ContextSwitcher.Database;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class SignUpController : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;
    private readonly IConfiguration _config;
    private readonly Database _database;

    public SignUpController(ILogger<AuthenticationController> logger, IConfiguration config, Database database)
    {
        _logger = logger;
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _database = database;
    }


    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    public IActionResult SignUp([FromForm]UserLoginRequestModel userSignUpModel)
    {
        _database.CreateUser(new UserModel()
        {
            Username = userSignUpModel.username,
            Password = userSignUpModel.passw,
            IsAdmin = false
        });

        return Ok("Successful");
    }
}