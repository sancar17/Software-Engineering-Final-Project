using System;
using System.Diagnostics;
using Microsoft.Office.Interop.Word;

namespace ContextSwitcher.Plugins
{
    public class WordPlugin : Plugin
    {

        private Document _document;
        
        public WordPlugin(PluginType pluginType, string filePath, int accessLevel) : base(pluginType, filePath, accessLevel)
        {
        }


        public override bool Open(int userId)
        {
            if (!base.Open(userId)) return false; // If the user has not been granted access

            Application application = new Application
            {
                Visible = true
            };

            try
            {
                _document = application.Documents.Open(FilePath);
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
                _document.Close();
            }
            catch (Exception e)
            {
                // ignored
            }
            
            Process[] wordProcesses = Process.GetProcessesByName("WINWORD");

            foreach (Process chromeProcess in wordProcesses)
            {
                chromeProcess.CloseMainWindow();
            }


            return true;
        }


        protected override void Save()
        {
            base.Save();
            try
            {
                _document.Save();            
            }
            catch (Exception e)
            {
                // ignored
            }
            
        }
    }
    
    
    
}