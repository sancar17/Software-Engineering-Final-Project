using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using BackendClient.Api;

namespace ContextSwitcher
{
    public class BackendManager
    {
        private static BackendManager _instance;  
        public static BackendManager Instance {  
            get { return _instance ??= new BackendManager(); }  
        }
        
        private readonly BackendClient.Api.BackendClient _backendClient;

        public BackendManager()
        {
            _backendClient = new BackendClient.Api.BackendClient("http://localhost:5000", new HttpClient());
        }


        public async System.Threading.Tasks.Task Authenticate(string username, string passw)
        {
            UserLoginResponseModel response = await _backendClient.AuthenticationPOSTAsync(new UserLoginRequestModel()
            {
                Username = username,
                Passw = passw
            });

            TokenStorage.Instance.Token = response.Token;
            TokenStorage.Instance.UserId = response.UserId;
        }


        public async System.Threading.Tasks.Task GetTasksAndPlugins(int userId)
        {
            TaskManager.Instance.TaskList.Clear();
            TaskManager.Instance.PluginDict.Clear();
            
            await GetTasks(userId);

            foreach (TaskModel task in TaskManager.Instance.TaskList)
            {
                await GetPlugins(TokenStorage.Instance.UserId, task.TaskId);
            }
        }


        public async System.Threading.Tasks.Task GetTasks(int userId)
        {
            var response = await _backendClient.GetTasksAsync(userId);
            
            Console.WriteLine("TASKS: ");

            foreach (var task in response)
            {
                Console.WriteLine(task.Description + " " + task.Title + " " + task.TaskId);
            }

            TaskManager.Instance.TaskList = response.ToList();
        }


        public async System.Threading.Tasks.Task GetPlugins(int userId, int taskId)
        {
            var response = await _backendClient.GetPluginsAsync(taskId, userId);

            if (response is null) return;
            
            Console.WriteLine("PLUGINS FOR TASK ID : " + taskId + " size: " + response.Count);
            
            foreach (var plugin in response)
            {
                Console.WriteLine(plugin.FilePath + " " + plugin.PluginType);
            }

            TaskManager.Instance.PluginDict.Add(taskId, response.ToList());

        }


        public async System.Threading.Tasks.Task CreateTask(int userId, string title, string description, List<PluginModel> pluginList)
        {
            await _backendClient.CreateTaskAsync(new TaskCreateRequestModel()
            {
                Title = title,
                Description = description,
                UserId = userId,
                PluginList = pluginList
            });
        }


        public async System.Threading.Tasks.Task SignUp(string username, string passw)
        {
            await _backendClient.SignUpAsync(username, passw);
        }


        public async System.Threading.Tasks.Task DeleteTask(int taskId)
        {
            await _backendClient.DeleteTaskAsync(taskId);
        }


        public async System.Threading.Tasks.Task AdminRequest(string username)
        {
            await _backendClient.RequestAsync(username);
        }


        public async System.Threading.Tasks.Task<List<string>> GetAdminRequests()
        {
            return new List<string>(await _backendClient.GetAdminRequestsAsync());
        }


        public async System.Threading.Tasks.Task AcceptAdminRequest(string username)
        {
            await _backendClient.AcceptAdminRequestAsync(username);
        }
        
        
        public async System.Threading.Tasks.Task DeclineAdminRequest(string username)
        {
            await _backendClient.DeclineAdminRequestAsync(username);
        }


        public async System.Threading.Tasks.Task<List<int>> GetAllEmployees()
        {
            return _backendClient.GetEmployeesAsync().Result.ToList();
        }

    }
}