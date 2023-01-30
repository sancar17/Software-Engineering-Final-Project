using Backend.Models;
using ContextSwitcher.Database;
using ContextSwitcher.Plugins;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Backend.Controllers;

[ApiController]
[Route("[controller]")]
public class TaskController : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;
    private readonly IConfiguration _config;
    private readonly Database _database;

    public TaskController(ILogger<AuthenticationController> logger, IConfiguration config, Database database)
    {
        _logger = logger;
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _database = database;
    }


    [HttpGet]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TaskModel>))]
    [Produces("application/json")]
    [Route("GetTasks")]
    public IActionResult GetTasks(int userId)
    {
        // TODO : Check if the authenticated user has access to this userId
        List<TaskModel> tasks = _database.ReadTasksOf(userId).Result;
        return new OkObjectResult(tasks);
    }


    [HttpPost]
    [Authorize]
    [Produces("application/json")]
    [Route("CreateTask")]
    public IActionResult CreateTask([FromBody]TaskCreateRequestModel model)
    {
        _database.CreateTask(model.Title, model.Description, model.UserId);

        int taskId = _database.ReadTasksOf(model.UserId).Result.FirstOrDefault(x => x.Title == model.Title)!.TaskId;
        
        foreach (PluginModel plugin in model.PluginList)
        {
            _database.CreatePlugin(plugin.PluginType, plugin.FilePath, taskId);
        }
        return Ok("Created");
    }


    [HttpDelete]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    [Route("DeleteTask")]
    public IActionResult DeleteTask(int taskId)
    {
        int loggedInUserId = 
            Int32.Parse(HttpContext.User.Claims.FirstOrDefault(x => x.Type == "id")?.Value ?? "-1");
        if (loggedInUserId == -1)
        {
            return new NotFoundResult();
        }

        try
        {
            _database.DeleteTask(taskId, loggedInUserId);
        }
        catch (DoesNotHaveAccessException)
        {
            return new UnauthorizedResult();
        }
        

        return Ok("Deleted");
    }
}