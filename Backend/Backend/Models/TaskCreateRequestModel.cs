using ContextSwitcher.Plugins;

namespace Backend.Models;

public class TaskCreateRequestModel
{
    public int UserId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<PluginModel> PluginList { get; set; }
}