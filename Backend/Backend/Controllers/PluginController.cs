using ContextSwitcher.Database;
using ContextSwitcher.Plugins;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class PluginController : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;
    private readonly IConfiguration _config;
    private readonly Database _database;

    public PluginController(ILogger<AuthenticationController> logger, IConfiguration config, Database database)
    {
        _logger = logger;
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _database = database;
    }


    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PluginModel>))]
    [Produces("application/json")]
    [Route("GetPlugins")]
    public IActionResult GetPlugins(int taskId, int userId)
    {
        string value = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;

        if (value == null) return new UnauthorizedResult();
        
        int loggedInUserId = Int32.Parse(value);
        List<PluginModel> plugins = _database.ReadPluginsOf(taskId, loggedInUserId).Result;
        return new OkObjectResult(plugins);
    }
}