using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Models;
using ContextSwitcher.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{

    private readonly ILogger<AuthenticationController> _logger;
    private readonly IConfiguration _config;
    private readonly Database _database;

    public AuthenticationController(ILogger<AuthenticationController> logger, IConfiguration config, Database database)
    {
        _logger = logger;
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _database = database;
    }
    
    
    /// <summary>
    /// Authentication conbtroller
    /// </summary>
    /// <param name="payload"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserLoginResponseModel))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [Produces("application/json")]
    public IActionResult Login([FromBody]UserLoginRequestModel payload)
    {
        KeyValuePair<UserModel, string> kvp = GenerateJSONWebToken(payload);
        string token = kvp.Value;
        UserModel userModel = kvp.Key;
        
        if (string.IsNullOrWhiteSpace(token))
        {
            return new UnauthorizedResult();
        }

        return new OkObjectResult(new UserLoginResponseModel()
        {
            Token = token,
            UserId = userModel.UserId
        });
    }
    
    private KeyValuePair<UserModel, string> GenerateJSONWebToken(UserLoginRequestModel userInfo)
    {
        UserModel userModel = _database.Authenticate(userInfo).Result;

        if (userModel is null)
        {
            return new KeyValuePair<UserModel, string>(null, "");
        }
        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));    
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);    
    
        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Issuer"],    
            new List<Claim>
            {
                new Claim("id", userModel.UserId.ToString()),
                new Claim("isAdmin", userModel.IsAdmin.ToString()),
            },    
            expires: DateTime.Now.AddMinutes(120),    
            signingCredentials: credentials);    
    
        return new KeyValuePair<UserModel, string>(userModel, new JwtSecurityTokenHandler().WriteToken(token));    
    }


    [HttpGet]
    [Authorize]
    [Produces("application/json")]
    public IActionResult WhoAmI()
    {
        string value = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
        if (value != null)
        {
            int userId = Int32.Parse(value);
            Console.WriteLine("ID:" + userId);
            
            return Ok("hello my name is ehakan " + userId);
        }

        return Ok("Failed");
    }

}