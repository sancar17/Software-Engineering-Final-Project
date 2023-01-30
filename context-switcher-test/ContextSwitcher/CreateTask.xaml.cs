using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ContextSwitcher;
using BackendClient.Api;

namespace ContextSwitcherTest
{
    public partial class CreateTask : Window
    {
        private string[] pluginTypes = new[]
        {
            "-",
            "excel",
            "word",
            "notepad++",
            "powerpoint",
            "chrome",
        };
        
        public CreateTask()
        {
            InitializeComponent();
        }

        private void OpenFileButton_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dialog.FileName;
                int pluginNumber = int.Parse(((Button)sender).Tag.ToString());
                if (FindName($"Plugin{pluginNumber}Path") is TextBox pluginPath) pluginPath.Text = filename;
            }
        }

        private async void Create_OnClick(object sender, RoutedEventArgs e)
        {
            string title = (FindName("Title") as TextBox)?.Text;
            string description = (FindName("Details") as TextBox)?.Text;

            List<PluginModel> pluginList = new List<PluginModel>();

            for (int i = 1; i <= 4; ++i)
            {
                string pluginType = pluginTypes[((FindName($"PluginType{i}") as ComboBox)!).SelectedIndex];
                string pluginPath = (FindName($"Plugin{i}Path") as TextBox)?.Text;

                if (pluginType == "-") continue;
                
                pluginList.Add(new PluginModel(){PluginType = pluginType, FilePath = pluginPath});
            }

            await BackendManager.Instance.CreateTask(TokenStorage.Instance.UserId, title, description, pluginList);

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void MenuComboBox_OnSelected(object sender, SelectionChangedEventArgs e)
        {
            ClientManager.Instance.Navigate(MenuComboBox.SelectedIndex, this);
        }
    }
}