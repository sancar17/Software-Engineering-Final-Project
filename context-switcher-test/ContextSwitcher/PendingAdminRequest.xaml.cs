using System.Windows;
using ContextSwitcher;

namespace ContextSwitcherTest
{
    public partial class PendingAdminRequest : Window
    {
        public PendingAdminRequest()
        {
            InitializeComponent();
        }

        private async void ShowMessageBox_Click(object sender, RoutedEventArgs e)
        {
            string username = ClientManager.Instance.PendingAdminRequests.Dequeue();
            string msgtext = $"User {username}  wants to be your Admin";
            string txt = "Pending Admin Request";
            MessageBoxButton button = MessageBoxButton.YesNoCancel;
            MessageBoxResult result = MessageBox.Show(msgtext, txt, button);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    await BackendManager.Instance.AcceptAdminRequest(username);
                    break;
                case MessageBoxResult.No:
                    await BackendManager.Instance.DeclineAdminRequest(username);
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }

            if (ClientManager.Instance.PendingAdminRequests.Count == 0)
            {
                Close();
            }
        }
    }
}