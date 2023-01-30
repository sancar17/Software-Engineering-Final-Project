using ContextSwitcher;

namespace ContextSwitcher.Plugins
{
    public abstract class Plugin
    {
        protected PluginType PluginType;
        protected readonly string FilePath;
        protected int AccessLevel;

        public bool IsOpen;


        public Plugin(PluginType pluginType, string filePath, int accessLevel)
        {
            PluginType = pluginType;
            FilePath = filePath;
            AccessLevel = accessLevel;
        }


        /// <summary>
        /// Opens the plugin, it first checks for the access using the given user ID parameter
        /// </summary>
        /// <param name="userId">user's id that is trying to access currently</param>
        public virtual bool Open(int userId)
        {
            if (!CheckAccess(userId)) return false;
            IsOpen = true;
            return true;
        }


        /// <summary>
        /// Closes the plugin application
        /// </summary>
        public virtual bool Close()
        {
            if (!IsOpen) return false;
            Save();
            return true;
        }



        /// <summary>
        /// Saves the current progress for the plugin
        /// </summary>
        protected virtual void Save()
        {
            
        }


        /// <summary>
        /// Checks if the given user has access to this plugin action
        /// </summary>
        /// <param name="userId">user's id that is trying to access currently</param>
        /// <returns>true if the user has access, false otherwise</returns>
        private bool CheckAccess(int userId)
        {
            return true;
        }
    }
}