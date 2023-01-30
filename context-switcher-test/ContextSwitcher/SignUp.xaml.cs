using System.Windows;
using ContextSwitcher;

namespace ContextSwitcherTest
{
    public partial class SignUp : Window
    {
        public SignUp()
        {
            InitializeComponent();
        }

        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            await BackendManager.Instance.SignUp(UserTextBox.Text, PasswordBox.Password);
            Login loginWindow = new Login();
            loginWindow.Show();
            Close();
        }
    }
}