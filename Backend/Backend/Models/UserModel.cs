namespace Backend.Models
{
    public class UserModel
    {
        public int UserId { get; set; }
        public bool IsAdmin { get; set; }

        public string Username;
        public string Password;
        
        public int AdminId;
        
        public List<TaskModel> TaskList;

        public UserModel()
        {
            
        }

        public UserModel(int userId, bool isAdmin, int adminId, ContextSwitcher.Database.Database database)
        {
            UserId = userId;
            IsAdmin = isAdmin;
            AdminId = adminId;
            // TODO : get tasks of this user 
        }
    }
}
