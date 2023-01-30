using ContextSwitcher.Database;
using ContextSwitcher.Plugins;

namespace Backend.Models
{
    public class TaskModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int TaskId { get; set; }

        public int OwnerId;
        public List<int> PluginList;


        public TaskModel(string title, string description, int taskId, int ownerId)
        {
            TaskId = taskId;
            Title = title;
            Description = description;
            OwnerId = ownerId;
        }
    }
}