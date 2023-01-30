using System;
using System.Collections.Generic;
using BackendClient.Api;
using ContextSwitcher.Plugins;
using Plugin = ContextSwitcher.Plugins.Plugin;


namespace ContextSwitcher
{
    public class TaskManager
    {
        private static TaskManager _instance;  
        public static TaskManager Instance {  
            get { return _instance ??= new TaskManager(); }  
        }

        public List<TaskModel> TaskList = new List<TaskModel>();
        public readonly Dictionary<int, List<PluginModel>> PluginDict = new Dictionary<int, List<PluginModel>>();
        public int ActiveTaskNumber = -1;
        public List<Plugin> ActivePlugins = new List<Plugin>();


        public void CloseTask()
        {
            if (ActiveTaskNumber == -1) return;

            foreach (Plugin activePlugin in ActivePlugins)
            {
                activePlugin.Close();
            }
            
            ActivePlugins.Clear();
            ActiveTaskNumber = -1;
        }


        public void OpenTask(int taskNumber)
        {
            CloseTask();

            ActiveTaskNumber = taskNumber;
             
            foreach (PluginModel plugin in PluginDict[TaskList[taskNumber].TaskId])
            {
                Console.WriteLine("type: " + plugin.PluginType);
                switch (plugin.PluginType)
                {
                    case "chrome":
                        ChromePlugin chromePlugin = new ChromePlugin(PluginType.CHROME, plugin.FilePath, 0);
                        chromePlugin.Open(1); // TODO : Remove this 
                        ActivePlugins.Add(chromePlugin);
                        break;
                    case "word":
                        WordPlugin wordPlugin = new WordPlugin(PluginType.WORD, plugin.FilePath, 0);
                        wordPlugin.Open(1);
                        ActivePlugins.Add(wordPlugin);
                        break;
                    case "excel":
                        ExcelPlugin excelPlugin = new ExcelPlugin(PluginType.EXCEL, plugin.FilePath, 0);
                        excelPlugin.Open(1);
                        ActivePlugins.Add(excelPlugin);
                        break;
                    case "powerpoint":
                        PowerpointPlugin powerpointPlugin =
                            new PowerpointPlugin(PluginType.POWERPOINT, plugin.FilePath, 0);
                        powerpointPlugin.Open(1);
                        ActivePlugins.Add(powerpointPlugin);
                        break;
                    case "notepad++":
                        NotepadppPlugin notepadppPlugin = new NotepadppPlugin(PluginType.NOTEPADPP, plugin.FilePath, 0);
                        notepadppPlugin.Open(1);
                        ActivePlugins.Add(notepadppPlugin);
                        break;
                    default:
                        break;
                }
            }
            
            Console.WriteLine(ActivePlugins.Count);
            
        }
        
    }
}