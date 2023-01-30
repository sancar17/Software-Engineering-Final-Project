using System;
using System.Diagnostics;
using Microsoft.Office.Interop.Excel;

namespace ContextSwitcher.Plugins
{
    public class ExcelPlugin : Plugin
    {

        private Workbook _workbook;
        
        public ExcelPlugin(PluginType pluginType, string filePath, int accessLevel) : base(pluginType, filePath, accessLevel)
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
                _workbook = application.Workbooks.Open(FilePath);
            }
            catch (Exception e)
            {
                // ignored
            }


            return true;
        }


        protected override void Save()
        {
            base.Save();
            try
            {
                _workbook.Save();
            }
            catch (Exception e)
            {
                // ignored
            }
        }
        
        
        public override bool Close()
        {
            if (!base.Close()) return false; // If the plugin has not already been opened yet

            try
            {
                _workbook.Close();
            }
            catch (Exception e)
            {
                // ignored
            }
            
            Process[] excelProcesses = Process.GetProcessesByName("EXCEL");

            foreach (Process chromeProcess in excelProcesses)
            {
                chromeProcess.CloseMainWindow();
            }

            return true;
        }
        
    }
}