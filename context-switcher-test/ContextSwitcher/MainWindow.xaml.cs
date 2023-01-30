using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ContextSwitcher;

namespace ContextSwitcherTest
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            InitializeTasks();
            
            CheckForAdminRequests();
        }


        private async void CheckForAdminRequests()
        {
            List<string> pendingRequests = await BackendManager.Instance.GetAdminRequests();
            if (pendingRequests.Count == 0) return;
            foreach (string pendingRequest in pendingRequests)
            {
                ClientManager.Instance.PendingAdminRequests.Enqueue(pendingRequest);
            }

            PendingAdminRequest pendingAdminRequestWindow = new PendingAdminRequest();
            pendingAdminRequestWindow.Show();
        }


        private async void InitializeTasks()
        {
            await BackendManager.Instance.GetTasksAndPlugins(TokenStorage.Instance.UserId);
            
            for (int i = 0; i <= 5; ++i)
            {
                string descriptionName = $"Task{i+1}Description";
                string titleName = $"Task{i+1}Title";
                string userControl = $"Task{i+1}Grid";
                
                TextBlock descriptionTextBlock = FindName(descriptionName) as TextBlock;
                Label titleLabel = FindName(titleName) as Label;
                UserControl gridUserControl = FindName(userControl) as UserControl;

                if (i < TaskManager.Instance.TaskList.Count)
                {
                    if (descriptionTextBlock != null)
                    {
                        descriptionTextBlock.Text = TaskManager.Instance.TaskList[i].Description;
                    }

                    if (titleLabel != null)
                    {
                        titleLabel.Content = TaskManager.Instance.TaskList[i].Title;
                    }
                }
                else
                {
                    gridUserControl.Visibility = Visibility.Hidden;
                    if (descriptionTextBlock != null) descriptionTextBlock.Text = "EMPTY TASK";
                    if (titleLabel != null) titleLabel.Content = "EMPTY TASK";
                }
            }

        }
        

        private void Switch_OnClick(object sender, RoutedEventArgs e)
        {
            int taskNumber = int.Parse(((Button)sender).Tag.ToString()) - 1; // because it is 1-indexed;

            if (TaskManager.Instance.ActiveTaskNumber != -1)
            {
                ((FindName($"Switch{TaskManager.Instance.ActiveTaskNumber+1}") as Button)!).Content = "Switch";
            }
            
            ((FindName($"Switch{taskNumber+1}") as Button)!).Content = "Active";
            
            TaskManager.Instance.OpenTask(taskNumber);
        }

        private void CreateTask_OnClick(object sender, RoutedEventArgs e)
        {
            CreateTask createTaskWindow = new CreateTask();
            createTaskWindow.Show();
            Close();
        }

        private async void DeleteTask6_OnClick(object sender, RoutedEventArgs e)
        {
            await BackendManager.Instance.DeleteTask(TaskManager.Instance.TaskList[5].TaskId);
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private async void DeleteTask5_OnClick(object sender, RoutedEventArgs e)
        {
            await BackendManager.Instance.DeleteTask(TaskManager.Instance.TaskList[4].TaskId);
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private async void DeleteTask4_OnClick(object sender, RoutedEventArgs e)
        {
            await BackendManager.Instance.DeleteTask(TaskManager.Instance.TaskList[3].TaskId);
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private async void DeleteTask3_OnClick(object sender, RoutedEventArgs e)
        {
            await BackendManager.Instance.DeleteTask(TaskManager.Instance.TaskList[2].TaskId);
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private async void DeleteTask2_OnClick(object sender, RoutedEventArgs e)
        {
            await BackendManager.Instance.DeleteTask(TaskManager.Instance.TaskList[1].TaskId);
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private async void DeleteTask1_OnClick(object sender, RoutedEventArgs e)
        {
            await BackendManager.Instance.DeleteTask(TaskManager.Instance.TaskList[0].TaskId);
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void MenuComboBox_OnSelected(object sender, RoutedEventArgs e)
        {
            ClientManager.Instance.Navigate(MenuComboBox.SelectedIndex, this);
        }
    }
}