using Backend.Models;
using ContextSwitcher.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class AdminController : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;
    private readonly IConfiguration _config;
    private readonly Database _database;

    public AdminController(ILogger<AuthenticationController> logger, IConfiguration config, Database database)
    {
        _logger = logger;
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _database = database;
    }


    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    [Route("Request")]
    public IActionResult AddAdminRequest(string username)
    {
        string value = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
        if (value != null)
        {
            int userId = Int32.Parse(value);
            
            _database.AdminAddUserRequest(userId, username);
        }

        return Ok("Successful");
    }
    
    
    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<string>))]
    [Produces("application/json")]
    [Route("GetAdminRequests")]
    public IActionResult GetAdminRequests()
    {
        List<string> requests = new List<string>(); 
        
        string value = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
        if (value != null)
        {
            int userId = Int32.Parse(value);

            List<int> adminCandidateId = _database.ReadRequests(userId).Result;
            foreach (int i in adminCandidateId)
            {
                Console.WriteLine("ID: " + i + "username: " + _database.ReadUser(i).Result.Username);
                requests.Add(_database.ReadUser(i).Result.Username);
            }
        }

        return new OkObjectResult(requests);
    }


    [HttpGet]
    [Authorize]
    [Produces("application/json")]
    [Route("AcceptAdminRequest")]
    public IActionResult AcceptAdminRequest(string username)
    {
        string value = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
        if (value != null)
        {
            int loggedInUserId = Int32.Parse(value);
            int adminId = 0;

            try
            {
                adminId = _database.ReadUser(username).Result.UserId;
            }
            catch (NullReferenceException)
            {
                return Unauthorized();
            }

            _database.SetAdminUserConnection(adminId, loggedInUserId);

            return Ok("Accepted");

        }
        
        
        

        return Unauthorized();
    }
    
    
    [HttpGet]
    [Authorize]
    [Produces("application/json")]
    [Route("DeclineAdminRequest")]
    public IActionResult DeclineAdminRequest(string username)
    {
        string value = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
        if (value != null)
        {
            int loggedInUserId = Int32.Parse(value);
            int adminId = 0;

            try
            {
                adminId = _database.ReadUser(username).Result.UserId;
            }
            catch (NullReferenceException)
            {
                return Unauthorized();
            }

            _database.ClearAdminUserConnection(adminId, loggedInUserId);

            return Ok("Accepted");

        }

        return Unauthorized();
    }


    [HttpGet]
    [Authorize]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<int>))]
    [Route("GetEmployees")]
    public IActionResult GetEmployees()
    {
        string value = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
        if (value != null)
        {
            int loggedInUserId = Int32.Parse(value);
            List<int> ids = _database.ReadUsersOf(loggedInUserId).Result;
            return new OkObjectResult(ids);
        }

        return Unauthorized();

        
    }
}