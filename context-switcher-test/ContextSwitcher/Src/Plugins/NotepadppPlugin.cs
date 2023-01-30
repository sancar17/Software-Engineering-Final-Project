using System.Diagnostics;

namespace ContextSwitcher.Plugins
{
    public class NotepadppPlugin : Plugin
    {
        public NotepadppPlugin(PluginType pluginType, string filePath, int accessLevel) : base(pluginType, filePath, accessLevel)
        {
        }


        public override bool Open(int userId)
        {
            if (!base.Open(userId)) return false;

            // Code that will open unopened tabs
            Process.Start("notepad.exe", @FilePath);

            return true;
        }


        public override bool Close()
        {
            if (!base.Close()) return false;

            Process[] notepadppProcesses = Process.GetProcessesByName("notepad");

            foreach (Process notepadppProcess in notepadppProcesses)
            {
                // TODO : Try to understand if the current process tab contains the file path or not
                notepadppProcess.CloseMainWindow();
            }

            return true;
        }
    }
}