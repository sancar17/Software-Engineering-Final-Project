using System;
using System.Collections.Generic;
using System.Windows;

namespace ContextSwitcherTest
{
    public class ClientManager
    {
        private static ClientManager _instance;

        public static ClientManager Instance
        {
            get { return _instance ??= new ClientManager(); }
        }

        public Queue<string> PendingAdminRequests = new Queue<string>();

        /*
         *<ComboBoxItem Visibility="Collapsed">Menu</ComboBoxItem>
                    <ComboBoxItem>Main Window</ComboBoxItem>
                    <ComboBoxItem>Edit Task</ComboBoxItem>
                    <ComboBoxItem>Employee Task</ComboBoxItem>
                    <ComboBoxItem>Add Employee</ComboBoxItem>
                    <ComboBoxItem>Profile</ComboBoxItem>
                    <ComboBoxItem>Logout</ComboBoxItem>
         * 
         */
        public void Navigate(int windowIndex, Window oldWindow)
        {
            switch (windowIndex)
            {
                case 1:
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    oldWindow.Close();
                    break;
                case 2:
                    break;
                case 3:
                    EmployeeTasks employeeTasksWindow = new EmployeeTasks();
                    employeeTasksWindow.Show();
                    oldWindow.Close();
                    break;
                case 4:
                    AdminRequest adminRequestWindow = new AdminRequest();
                    adminRequestWindow.Show();
                    oldWindow.Close();
                    break;
                case 5:
                    break;
                case 6:
                    oldWindow.Close();
                    break;
                default:
                    break;
            }
        }
    }
}