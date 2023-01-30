using System;
using System.Diagnostics;
using System.Windows.Automation;

namespace ContextSwitcher.Plugins
{
    public class ChromePlugin : Plugin
    {
        public ChromePlugin(PluginType pluginType, string filePath, int accessLevel) : base(pluginType, filePath, accessLevel)
        {
        }


        public override bool Open(int userId)
        {
            if (!base.Open(userId)) return false;

            Process[] procsChrome = Process.GetProcessesByName("chrome");
            if (procsChrome.Length <= 0)
            {
                Console.WriteLine("Chrome is not running");
            }

            foreach (Process process in procsChrome)
            {
                if(process.MainWindowHandle == IntPtr.Zero) continue;
                
                AutomationElement root = AutomationElement.FromHandle(process.MainWindowHandle);
                Condition condition = new PropertyCondition(AutomationElement.ControlTypeProperty, ControlType.TabItem);
                var tabs = root.FindAll(TreeScope.Descendants, condition);
                foreach (AutomationElement tab in tabs)
                {
                    // TODO : Check if the chrome tab is already open or not
                    Console.WriteLine(tab.Current.Name);
                }
            }

            // Code that will open unopened tabs
            Process.Start("chrome.exe", FilePath);

            return true;
        }


        public override bool Close()
        {
            Console.WriteLine("Closing chrome!");
            if (!base.Close()) return false;
            
            Console.WriteLine("Passed this!");

            Process[] chromeProcesses = Process.GetProcessesByName("chrome");

            foreach (Process chromeProcess in chromeProcesses)
            {
                // TODO : Try to understand if the current process tab contains the file path or not
                chromeProcess.CloseMainWindow();
            }

            return true;
        }
    }
}