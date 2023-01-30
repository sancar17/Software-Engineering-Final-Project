using System.Windows;
using ContextSwitcher;

namespace ContextSwitcherTest
{
    public partial class AdminRequest : Window
    {
        public AdminRequest()
        {
            InitializeComponent();
        }

        private async void AdminRequest_OnClick(object sender, RoutedEventArgs e)
        {
            await BackendManager.Instance.AdminRequest(UserTextBox.Text);
            ClientManager.Instance.Navigate(1, this);
        }
    }
}