using System.Runtime.Serialization;
using Backend.Models;
using ContextSwitcher.Plugins;
using Npgsql;

namespace ContextSwitcher.Database
{

    [Serializable]
    public class DoesNotHaveAccessException : Exception
    {
        public DoesNotHaveAccessException()
        {
        }

        public DoesNotHaveAccessException(string message) : base(message)
        {
        }

        public DoesNotHaveAccessException(string message, Exception inner) : base(message, inner)
        {
        }

        protected DoesNotHaveAccessException(
            SerializationInfo info,
            StreamingContext context) : base(info, context)
        {
        }
    }
    
    // FOR REFERENCE: https://github.com/npgsql/npgsql
    public class Database
    {
        private static Database _instance;  
        public static Database Instance {  
            get { return _instance ??= new Database(); }  
        }

        private readonly string _connectionString =
            "Host=ec2-54-170-163-224.eu-west-1.compute.amazonaws.com;Username=qgphgeodomuuhp;Password=c3d3afcefe0b9908f58f1e0049603f89e27bf75f6940381e806719efa409069a;Database=dehkvum3jinn5m;Maximum Pool Size=20;Connection Idle Lifetime=20;ConnectionLifetime=20";


        private NpgsqlConnection _conn
        {
            get
            {
                Console.WriteLine(_connectionString);
                NpgsqlConnection con = new NpgsqlConnection(_connectionString);
                con.Open();
                return con;
            }
        }


        public async Task<UserModel> Authenticate(UserLoginRequestModel loginModel)
        {
            string sql = "SELECT * FROM users WHERE(username=@username AND passw=@passw) LIMIT 1";
            
            int id, adminId;
            bool isAdmin;
            
            await using (var cmd = new NpgsqlCommand(sql, _conn))
            {
                cmd.Parameters.AddWithValue("@username", loginModel.username);
                cmd.Parameters.AddWithValue("@passw", loginModel.passw);
                
                await using (var rdr = await cmd.ExecuteReaderAsync())
                {
                    if(rdr.Read()){
                        id = rdr.GetInt32(0);
                        isAdmin = rdr.GetBoolean(1);
                        adminId = rdr.IsDBNull(2) ? -1 : rdr.GetInt32(2);
                    }
                    else
                    {
                        if (cmd.Connection is not null)
                        {
                            await cmd.Connection.CloseAsync();
                        }
                        return null;
                    }
                }
                
                if (cmd.Connection is not null)
                {
                    await cmd.Connection.CloseAsync();
                }
                
            }

            UserModel newU = new UserModel(id, isAdmin, adminId, this);
            return newU;
        }


        public async void DeleteUser(string username)
        {
            string sql = "DELETE FROM users WHERE username=@username";
            await using var cmd = new NpgsqlCommand(sql, _conn); 
            cmd.Parameters.AddWithValue("@Username", username);
            await cmd.ExecuteNonQueryAsync();
            
            if (cmd.Connection is not null)
            {
                await cmd.Connection.CloseAsync();
            }
        }
        

        public async Task<UserModel> ReadUser(string username)
        {
            //open connection

            //selecting the userModel with userID

            string sql = "SELECT * FROM users WHERE(username=@username) LIMIT 1";
            
            int id, adminId;
            bool isAdmin;
            
            await using (var cmd = new NpgsqlCommand(sql, _conn))
            {
                cmd.Parameters.AddWithValue("@username", username);
                
                await using (var rdr = await cmd.ExecuteReaderAsync())
                {
                    if(rdr.Read()){
                        id = rdr.GetInt32(0);
                        isAdmin = rdr.GetBoolean(1);
                        adminId = rdr.IsDBNull(2) ? -1 : rdr.GetInt32(2);
                    }
                    else
                    {
                        if (cmd.Connection is not null)
                        {
                            await cmd.Connection.CloseAsync();
                        }
                        return null;
                    }
                    if (cmd.Connection is not null)
                    {
                        await cmd.Connection.CloseAsync();
                    }
                }
            }

            UserModel newU = new UserModel(id, isAdmin, adminId, this);
            return newU;
        }
        

        /// <summary>
        /// Do a lookup based on userModel id
        /// Write query results into a userModel object and return
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>userModel object</returns>
        public async Task<UserModel> ReadUser(int userId)
        {
            //open connection

            //selecting the userModel with userID

            string sql = "SELECT * FROM users WHERE(userid=@userId) LIMIT 1";
            
            int id, adminId;
            bool isAdmin;
            string username;
            
            await using (var cmd = new NpgsqlCommand(sql, _conn))
            {
                cmd.Parameters.AddWithValue("@userid", userId);
                
                await using (var rdr = await cmd.ExecuteReaderAsync())
                {
                    if(rdr.Read()){
                        id = rdr.GetInt32(0);
                        isAdmin = rdr.GetBoolean(1);
                        adminId = rdr.IsDBNull(2) ? -1 : rdr.GetInt32(2);
                        username = rdr.GetString(3);
                    }
                    else
                    {
                        if (cmd.Connection is not null)
                        {
                            await cmd.Connection.CloseAsync();
                        }
                        Console.WriteLine("THIS ID RETURNS NULL : " + userId);
                        return null;
                    }
                    if (cmd.Connection is not null)
                    {
                        await cmd.Connection.CloseAsync();
                    }
                }
            }

            UserModel newU = new UserModel(id, isAdmin, adminId, this);
            newU.Username = username;
            return newU;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userModel"></param>
        /// <exception cref="NotImplementedException"></exception>
        public async void CreateUser(UserModel userModel)
        {
            await using var cmd = new NpgsqlCommand();
            cmd.Connection = _conn;

            //add new userModel to database

            cmd.CommandText = "INSERT INTO users(isadmin, passw, username) VALUES(@isadmin, @passw, @username)";
            cmd.Parameters.AddWithValue("@username", userModel.Username);
            cmd.Parameters.AddWithValue("@isadmin", userModel.IsAdmin);
            cmd.Parameters.AddWithValue("@passw", userModel.Password);
            await cmd.ExecuteNonQueryAsync();
            
            if (cmd.Connection is not null)
            {
                await cmd.Connection.CloseAsync();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<TaskModel>> ReadTasksOf(int userId)
        {
            //read all tasks from database

            const string sql = "SELECT * FROM tasks WHERE(owner = @userId)";
            
            int id, ownerId;
            string title, description;
            List<TaskModel> tasks = new List<TaskModel>();

            await using var cmd = new NpgsqlCommand(sql, _conn);
            cmd.Parameters.AddWithValue("@userId", userId);
                
            await using var rdr = await cmd.ExecuteReaderAsync();
            while (rdr.Read())
            {
                id = rdr.GetInt32(0);
                title = rdr.GetString(1);
                description = rdr.GetString(2);
                ownerId = rdr.GetInt32(3);

                TaskModel newT = new(title, description, id, ownerId);
                tasks.Add(newT);
            }

            await rdr.CloseAsync();
            
            if (cmd.Connection is not null)
            {
                await cmd.Connection.CloseAsync();
            }

            return tasks;
            
        }

        
        /// <summary>
        /// Do a lookup based on task id
        /// Write query results into a task object and return
        /// </summary>
        /// <returns>task object</returns>
        public async Task<TaskModel> ReadTask(string title)
        {
            //selecting the task with taskID

            const string sql = "SELECT * FROM tasks WHERE (title = @title)";
            
            await using var cmd = new NpgsqlCommand(sql, _conn);
            
            cmd.Parameters.AddWithValue("@title", title);

            await using NpgsqlDataReader rdr = await cmd.ExecuteReaderAsync();

            string description;
            int ownerId, taskId;

            if(rdr.Read())
            {
                taskId = rdr.GetInt32(0);
                description = rdr.GetString(2);
                ownerId = rdr.GetInt32(3);
            }
            else
            {
                if (cmd.Connection is not null)
                {
                    await cmd.Connection.CloseAsync();
                }
                return null;
            }

            await rdr.CloseAsync();
            
            if (cmd.Connection is not null)
            {
                await cmd.Connection.CloseAsync();
            }

            TaskModel newT = new TaskModel(title, description, taskId, ownerId);
            return newT;
        }

        
        
        /// <summary>
        /// Do a lookup based on task id
        /// Write query results into a task object and return
        /// </summary>
        /// <returns>task object</returns>
        private async Task<TaskModel> ReadTask(int taskId)
        {
            //selecting the task with taskID

            const string sql = "SELECT * FROM tasks WHERE (TaskId = @id)";
            
            await using var cmd = new NpgsqlCommand(sql, _conn);
            
            cmd.Parameters.AddWithValue("@id", taskId);

            await using NpgsqlDataReader rdr = await cmd.ExecuteReaderAsync();

            string description, title;
            int ownerId;

            if(rdr.Read())
            {
                title = rdr.GetString(1);
                description = rdr.GetString(2);
                ownerId = rdr.GetInt32(3);
            }
            else
            {
                if (cmd.Connection is not null)
                {
                    await cmd.Connection.CloseAsync();
                }
                return null;
            }

            await rdr.CloseAsync();
            
            if (cmd.Connection is not null)
            {
                await cmd.Connection.CloseAsync();
            }

            TaskModel newT = new TaskModel(title, description, taskId, ownerId);
            return newT;
        }

        /// <summary>
        /// Edits the specific task described with the task id integer value
        /// with given parameters
        /// </summary>
        /// <param name="taskId">id of the task to be edited</param>
        /// <param name="pluginList">new plugin list</param>
        /// <param name="newTitle"></param>
        /// <param name="newDescription"></param>
        /// <param name="userId"></param>
        public async void EditTask(int taskId, List<PluginModel> pluginList, string newTitle, string newDescription, int userId)
        {
            if (await CheckForAccess(taskId, userId) == false)
            {
                throw new DoesNotHaveAccessException();
            }
            
            await using var cmd = new NpgsqlCommand();
            cmd.Connection = _conn;

            //edit task

            cmd.CommandText = "UPDATE tasks SET title = @new_title, description = @new_description WHERE (taskId = @id)";
            cmd.Parameters.AddWithValue("@new_title", newTitle);
            cmd.Parameters.AddWithValue("@new_description", newDescription);
            cmd.Parameters.AddWithValue("@id", taskId);
            await cmd.ExecuteNonQueryAsync();

            //remove plugins of this task from database

            cmd.CommandText = "DELETE FROM plugins WHERE (task = @id)";
            cmd.Parameters.AddWithValue("@id", taskId);
            await cmd.ExecuteNonQueryAsync();

            //add new plugins to database

            foreach (PluginModel plugin in pluginList)
            {
                cmd.CommandText = "INSERT INTO plugins(pluginid, filepath, accesslevel, task) VALUES(@id, @path, @level, @tid)";
                cmd.Parameters.AddWithValue("@id", plugin.PluginId);
                cmd.Parameters.AddWithValue("@path", plugin.FilePath);
                cmd.Parameters.AddWithValue("@level", plugin.AccessLevel);
                cmd.Parameters.AddWithValue("@tid", taskId);
                await cmd.ExecuteNonQueryAsync();
            }
            
            if (cmd.Connection is not null)
            {
                await cmd.Connection.CloseAsync();
            }
        }

        public async void CreateTask(string title, string description, int owner)
        {
            await using var cmd = new NpgsqlCommand();
            cmd.Connection = _conn;

            //add new task to database

            cmd.CommandText = "INSERT INTO tasks(title, description, owner) VALUES(@TITLE, @DES, @OWN)";
            cmd.Parameters.AddWithValue("@TITLE", title);
            cmd.Parameters.AddWithValue("@DES", description);
            cmd.Parameters.AddWithValue("@OWN", owner);
            cmd.ExecuteNonQuery();
            
            if (cmd.Connection is not null)
            {
                await cmd.Connection.CloseAsync();
            }
        }


        public async Task<List<PluginModel>> ReadAllPlugins()
        {

            //read all plugins from database
            string type, path;
            int level, id;
            List<PluginModel> plugins = new List<PluginModel>();

            const string sql = "SELECT * FROM plugins";
            await using var cmd = new NpgsqlCommand(sql, _conn);

            await using var rdr = await cmd.ExecuteReaderAsync();
            while (rdr.Read())
            {
                id = rdr.GetInt32(0);
                path = rdr.GetString(1);
                level = rdr.GetInt32(2);
                type = rdr.GetString(4);

                PluginModel newP = new PluginModel();
                newP.PluginId = id;
                newP.FilePath = path;
                newP.AccessLevel = level;
                newP.PluginType = type;
                
                plugins.Add(newP);
            }
            
            await rdr.CloseAsync();
            
            if (cmd.Connection is not null)
            {
                await cmd.Connection.CloseAsync();
            }
            
            return plugins;
        }


        public async void CreatePlugin(string pluginType, string filePath, int taskId)
        {
            await using var cmd = new NpgsqlCommand();
            cmd.Connection = _conn;

            cmd.CommandText = "INSERT INTO plugins (filePath, task, pluginType) VALUES(@FILEPATH, @TASK, @PLUGINTYPE)";
            cmd.Parameters.AddWithValue("@FILEPATH", filePath);
            cmd.Parameters.AddWithValue("@TASK", taskId);
            cmd.Parameters.AddWithValue("@PLUGINTYPE", pluginType);
            await cmd.ExecuteNonQueryAsync();
            
            if (cmd.Connection is not null)
            {
                await cmd.Connection.CloseAsync();
            }
        }
        
        
        public async void DeletePlugin(int pluginId)
        {
            await using var cmd = new NpgsqlCommand();
            cmd.Connection = _conn;

            cmd.CommandText = "DELETE FROM plugins WHERE (pluginid = @id)";
            cmd.Parameters.AddWithValue("@id", pluginId);
            await cmd.ExecuteNonQueryAsync();
            
            if (cmd.Connection is not null)
            {
                await cmd.Connection.CloseAsync();
            }

        }


        /// <summary>
        /// Removes the task described with the given task id
        /// </summary>
        /// <param name="taskId">task to be removed</param>
        /// <param name="userId"></param>
        /// <exception cref="NotImplementedException"></exception>
        public async void DeleteTask(int taskId, int userId)
        {
            if (await CheckForAccess(taskId, userId) == false)
            {
                return;
            }

            await using var cmd = new NpgsqlCommand();
            cmd.Connection = _conn;
            
            cmd.CommandText = "DELETE FROM plugins WHERE (task = @id)";
            cmd.Parameters.AddWithValue("@id", taskId);
            cmd.ExecuteNonQuery();

            //delete row

            cmd.CommandText = "DELETE FROM tasks WHERE (TaskId = @id)";
            cmd.Parameters.AddWithValue("@id", taskId);
            cmd.ExecuteNonQuery();
            
            if (cmd.Connection is not null)
            {
                await cmd.Connection.CloseAsync();
            }
        }


        /// <summary>
        /// Check if the userModel has access to the given task
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task<bool> CheckForAccess(int taskId, int userId)
        {
            TaskModel taskModel = ReadTask(taskId).Result;

            if (taskModel is null) return true;
            
            if (taskModel.OwnerId == userId)
            {
                return true;
            }
            
            UserModel taskOwner = await ReadUser(taskModel.OwnerId);
            
            if (taskOwner is null) return true;

            return taskOwner.AdminId == userId;
        }
        
        
        /// <summary>
        /// Read a specific plugin by its id
        /// </summary>
        /// <returns></returns>
        public async Task<PluginModel> ReadPlugin(int taskId, string filePath)
        {
            //read all plugins from database
            string type, path;
            int level, id;
            
            PluginModel newP = new PluginModel();
            
            const string sql = "SELECT * FROM plugins WHERE(task = @taskId AND filepath = @filePath)";
            await using var cmd = new NpgsqlCommand(sql, _conn);
            cmd.Parameters.AddWithValue("@taskId", taskId);
            cmd.Parameters.AddWithValue("@filePath", filePath);
                
            await using var rdr = await cmd.ExecuteReaderAsync();
            if (rdr.Read())
            {
                id = rdr.GetInt32(0);
                path = rdr.GetString(1);
                level = rdr.GetInt32(2);
                type = rdr.GetString(4);

                
                newP.PluginId = id;
                newP.FilePath = path;
                newP.AccessLevel = level;
                newP.PluginType = type;
                
            }
            else
            {
                return null;
            }
            
            await rdr.CloseAsync();
            
            if (cmd.Connection is not null)
            {
                await cmd.Connection.CloseAsync();
            }
            
            return newP;
        }
        
        
        /// <summary>
        /// Read a specific plugin by its id
        /// </summary>
        /// <returns></returns>
        public async Task<PluginModel> ReadPlugin(int pluginId)
        {
            //read all plugins from database
            string type, path;
            int level, id;
            
            PluginModel newP = new PluginModel();


            const string sql = "SELECT * FROM plugins WHERE(pluginId = @pluginId)";
            await using var cmd = new NpgsqlCommand(sql, _conn);
            cmd.Parameters.AddWithValue("@pluginId", pluginId);
                
            await using var rdr = await cmd.ExecuteReaderAsync();
            if (rdr.Read())
            {
                id = rdr.GetInt32(0);
                path = rdr.GetString(1);
                level = rdr.GetInt32(2);
                type = rdr.GetString(4);

                
                newP.PluginId = id;
                newP.FilePath = path;
                newP.AccessLevel = level;
                newP.PluginType = type;
                
            }
            else
            {
                return null;
            }
            
            await rdr.CloseAsync();
            
            if (cmd.Connection is not null)
            {
                await cmd.Connection.CloseAsync();
            }
            
            return newP;
        }
        

        /// <summary>
        /// Read all plugins of specific task
        /// </summary>
        /// <returns></returns>
        public async Task<List<PluginModel>> ReadPluginsOf(int taskId, int userId)
        {
            // check for access

            if (await CheckForAccess(taskId, userId) == false)
            {
                throw new DoesNotHaveAccessException();
            }
            
            //read all plugins from database
            string type, path;
            int level, id;
            List<PluginModel> plugins = new List<PluginModel>();

            const string sql = "SELECT * FROM plugins WHERE(task = @taskId)";
            await using var cmd = new NpgsqlCommand(sql, _conn);
            cmd.Parameters.AddWithValue("@taskId", taskId);
                
            await using var rdr = await cmd.ExecuteReaderAsync();
            while (rdr.Read())
            {
                id = rdr.GetInt32(0);
                path = rdr.GetString(1);
                level = rdr.GetInt32(2);
                type = rdr.GetString(4);

                PluginModel newP = new PluginModel();
                newP.PluginId = id;
                newP.FilePath = path;
                newP.AccessLevel = level;
                newP.PluginType = type;
                
                plugins.Add(newP);
            }
            
            await rdr.CloseAsync();
            
            if (cmd.Connection is not null)
            {
                await cmd.Connection.CloseAsync();
            }
            
            return plugins;
        }
        
        
        //User sends request to another user to be their admin
        public async void AdminAddUserRequest(int adminId, string username)
        {
            await using var cmd = new NpgsqlCommand();
            cmd.Connection = _conn;
            
            Console.WriteLine("Username: " + username);

            int userId = -1;

            try
            {
                userId = ReadUser(username).Result.UserId;
            }
            catch (NullReferenceException e)
            {
                if (cmd.Connection is not null)
                {
                    await cmd.Connection.CloseAsync();
                }

                return;
            }
            

            cmd.CommandText = "INSERT INTO admin_requests (admin_candidate, \"user\") VALUES(@admin, @user)";
            cmd.Parameters.AddWithValue("@admin", adminId);
            cmd.Parameters.AddWithValue("@user", userId);
            await cmd.ExecuteNonQueryAsync();
            
            if (cmd.Connection is not null)
            {
                await cmd.Connection.CloseAsync();
            }

        } 

        //If other user accepts the request, set admin-user connection
        public async void SetAdminUserConnection(int adminId, int userId)
        {
            await using var cmd = new NpgsqlCommand();
            cmd.Connection = _conn;

            cmd.CommandText =
                "SELECT * FROM admin_requests WHERE admin_candidate=@adminId AND \"user\"=@userId LIMIT 1";
            cmd.Parameters.AddWithValue("@adminId", adminId);
            cmd.Parameters.AddWithValue("@userId", userId);
            await using var reader =  await cmd.ExecuteReaderAsync();
            if (!reader.HasRows) // non existing request
            {
                await reader.CloseAsync();
                return;
            }

            await reader.CloseAsync();

            //update admin
            cmd.CommandText = "UPDATE users SET isadmin = @true WHERE (userid = @adminId)";
            cmd.Parameters.AddWithValue("@true", true);
            cmd.Parameters.AddWithValue("@adminId", adminId);
            await cmd.ExecuteNonQueryAsync();

            //update user
            cmd.CommandText = "UPDATE users SET admin = @admin WHERE (userid = @userId)";
            cmd.Parameters.AddWithValue("@admin", adminId);
            cmd.Parameters.AddWithValue("@userId", userId);
            await cmd.ExecuteNonQueryAsync();

            //delete request from database
            cmd.CommandText = "DELETE FROM admin_requests WHERE (admin_candidate = @admin AND \"user\" = @user)";
            cmd.Parameters.AddWithValue("@admin", adminId);
            cmd.Parameters.AddWithValue("@user", userId);
            await cmd.ExecuteNonQueryAsync();
            
            if (cmd.Connection is not null)
            {
                await cmd.Connection.CloseAsync();
            }

        }
        
        
        public async void ClearAdminUserConnection(int adminId, int userId)
        {
            await using var cmd = new NpgsqlCommand();
            cmd.Connection = _conn;
            
            //delete request from database
            cmd.CommandText = "DELETE FROM admin_requests WHERE (admin_candidate = @admin AND \"user\" = @user)";
            cmd.Parameters.AddWithValue("@admin", adminId);
            cmd.Parameters.AddWithValue("@user", userId);
            await cmd.ExecuteNonQueryAsync();
            
            if (cmd.Connection is not null)
            {
                await cmd.Connection.CloseAsync();
            }

        }

        //Delete user from admin
        public async void DeleteUserFromAdmin(int userId)
        {
            await using var cmd = new NpgsqlCommand();
            cmd.Connection = _conn;

            cmd.CommandText = "UPDATE users SET admin = @admin WHERE (userid = @id)";
            cmd.Parameters.AddWithValue("@admin", null);
            cmd.Parameters.AddWithValue("@id", userId);
            await cmd.ExecuteNonQueryAsync();
            
            if (cmd.Connection is not null)
            {
                await cmd.Connection.CloseAsync();
            }

        }

        //List of the users of an admin
        public async Task<List<int>> ReadUsersOf(int adminId)
        {
            //read all users which have the adminId

            int userid;
            List<int> ids = new List<int>();

            const string sql = "SELECT * FROM users WHERE(admin = @adminid)";
            await using var cmd = new NpgsqlCommand(sql, _conn);
            cmd.Parameters.AddWithValue("@adminid", adminId);

            await using var rdr = await cmd.ExecuteReaderAsync();
            while (rdr.Read())
            {
                userid = rdr.GetInt32(0);

                ids.Add(userid);
            }

            await rdr.CloseAsync();
            
            if (cmd.Connection is not null)
            {
                await cmd.Connection.CloseAsync();
            }

            return ids;
        }

        public async void TransferTask(int newUserId, int taskId)
        {
            //update the task's owner userModel

            await using var cmd = new NpgsqlCommand();
            cmd.Connection = _conn;

            //edit task

            cmd.CommandText = "UPDATE tasks SET owner = @userid WHERE (taskId = @taskid)";
            cmd.Parameters.AddWithValue("@taskid", taskId);
            cmd.Parameters.AddWithValue("user_id", newUserId);
            await cmd.ExecuteNonQueryAsync();
            
            if (cmd.Connection is not null)
            {
                await cmd.Connection.CloseAsync();
            }
        }
        
        //Read the requests sent to the user
        public async Task<List<int>> ReadRequests(int userId)
        {
            List<int> requests = new List<int>();

            const string sql = "SELECT * FROM admin_requests WHERE(\"user\" = @id)";
            await using var cmd = new NpgsqlCommand(sql, _conn);
            cmd.Parameters.AddWithValue("@id", userId);

            await using var rdr = await cmd.ExecuteReaderAsync();
            while (rdr.Read())
            {
                int adminCandidateId = rdr.GetInt32(1);
                requests.Add(adminCandidateId);
            }

            return requests;
        }

        //find sender of the request
        public async Task<int> ReadSenderOfRequest(int requestId)
        {
            int adminId = -1;

            string sql = "SELECT * FROM admin_requests WHERE(reques_id = @id)";
            await using (var cmd = new NpgsqlCommand(sql, _conn))
            {
                cmd.Parameters.AddWithValue("@id", requestId);

                await using var rdr = await cmd.ExecuteReaderAsync();
                while (rdr.Read())
                {
                    adminId = rdr.GetInt32(1);
                }
            }

            return adminId;
        }

    }
}
