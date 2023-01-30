namespace ContextSwitcher
{
    public class TokenStorage
    {
        private static TokenStorage _instance;  
        public static TokenStorage Instance {  
            get { return _instance ??= new TokenStorage(); }  
        }

        public string Token { get; set; }
        public int UserId { get; set; }
    }
}