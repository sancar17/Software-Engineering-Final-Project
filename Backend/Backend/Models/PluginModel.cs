using ContextSwitcher;

namespace ContextSwitcher.Plugins
{
    public class PluginModel
    {
        public string PluginType { get; set; }
        public string FilePath { get; set; }
        public int AccessLevel;
        public int PluginId;
    }
}