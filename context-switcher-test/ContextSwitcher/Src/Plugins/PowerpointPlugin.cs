using System;
using System.Diagnostics;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;

namespace ContextSwitcher.Plugins
{
    public class PowerpointPlugin : Plugin
    {

        private Presentation _presentation;
        
        public PowerpointPlugin(PluginType pluginType, string filePath, int accessLevel) : base(pluginType, filePath, accessLevel)
        {
        }


        public override bool Open(int userId)
        {
            if (!base.Open(userId)) return false; // If the user has not been granted access

            Application application = new Application
            {
                Visible = MsoTriState.msoTrue
            };

            try
            {
                _presentation = application.Presentations.Open(FilePath);
            }
            catch (Exception e)
            {
                // ignored
            }


            return true;
        }


        public override bool Close()
        {
            if (!base.Close()) return false; // If the plugin has not already been opened yet

            try
            {
                _presentation.Close();
            }
            catch (Exception e)
            {
                // ignored
            }
            
            Process[] powerpointProcesses = Process.GetProcessesByName("POWERPNT");

            foreach (Process powerpointProcess in powerpointProcesses)
            {
                powerpointProcess.CloseMainWindow();
            }


            return true;
        }


        protected override void Save()
        {
            base.Save();
            try
            {
                _presentation.Save();            
            }
            catch (Exception e)
            {
                // ignored
            }
            
        }
    }
    
    
    
}