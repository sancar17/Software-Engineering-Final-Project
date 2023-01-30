using System.Windows;
using ContextSwitcher;
using BackendClient.Api;

namespace ContextSwitcherTest
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private async void Login_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                await BackendManager.Instance.Authenticate(UserTextBox.Text, PasswordBox.Password);
                MainWindow window = new MainWindow();
                window.Show();
                this.Close();
            }
            catch (ApiException exception)
            {
                GreetingOutput.Text = "Not authorized!";
            }
            
        }

        private void SignupPage_OnClick(object sender, RoutedEventArgs e)
        {
            SignUp signUpWindow = new SignUp();
            signUpWindow.Show();
            Close();
        }
    }
}