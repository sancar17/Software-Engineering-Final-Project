using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ContextSwitcher;

namespace ContextSwitcherTest
{
    public partial class EmployeeTasks : Window
    {
        public EmployeeTasks()
        {
            InitializeComponent();
            InitializeTasks();
        }
        
        private async void InitializeTasks()
        {
            List<int> employeeIds =  BackendManager.Instance.GetAllEmployees().Result;

            int i = 0;

            foreach (int employeeId in employeeIds)
            {
                await BackendManager.Instance.GetTasksAndPlugins(employeeId);
            
                for (; i <= 5; ++i)
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
            
            

        }

        private void MenuComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClientManager.Instance.Navigate(MenuComboBox.SelectedIndex, this);
        }

        private async void DeleteTask1_OnClick(object sender, RoutedEventArgs e)
        {
            await BackendManager.Instance.DeleteTask(TaskManager.Instance.TaskList[0].TaskId);
            EmployeeTasks employeeTasks = new EmployeeTasks();
            employeeTasks.Show();
            Close();
        }

        private async void DeleteTask2_OnClick(object sender, RoutedEventArgs e)
        {
            await BackendManager.Instance.DeleteTask(TaskManager.Instance.TaskList[1].TaskId);
            EmployeeTasks employeeTasks = new EmployeeTasks();
            employeeTasks.Show();
            Close();
        }

        private async void DeleteTask3_OnClick(object sender, RoutedEventArgs e)
        {
            await BackendManager.Instance.DeleteTask(TaskManager.Instance.TaskList[2].TaskId);
            EmployeeTasks employeeTasks = new EmployeeTasks();
            employeeTasks.Show();
            Close();
        }

        private async void DeleteTask4_OnClick(object sender, RoutedEventArgs e)
        {
            await BackendManager.Instance.DeleteTask(TaskManager.Instance.TaskList[3].TaskId);
            EmployeeTasks employeeTasks = new EmployeeTasks();
            employeeTasks.Show();
            Close();
        }

        private async void DeleteTask5_OnClick(object sender, RoutedEventArgs e)
        {
            await BackendManager.Instance.DeleteTask(TaskManager.Instance.TaskList[4].TaskId);
            EmployeeTasks employeeTasks = new EmployeeTasks();
            employeeTasks.Show();
            Close();
        }

        private async void DeleteTask6_OnClick(object sender, RoutedEventArgs e)
        {
            await BackendManager.Instance.DeleteTask(TaskManager.Instance.TaskList[5].TaskId);
            EmployeeTasks employeeTasks = new EmployeeTasks();
            employeeTasks.Show();
            Close();
        }
    }
}