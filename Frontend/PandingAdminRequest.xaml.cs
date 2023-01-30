using System.Windows;
using System.Windows;
using System.Windows.Controls;

namespace ContextSwitcher
{
    public partial class PandingAdminRequest : Window
    {
        public PandingAdminRequest()
        {
            InitializeComponent();
        }

        private void ShowMessageBox_Click(object sender, RoutedEventArgs e)
        {
            string msgtext = "User 1 wants to be your Admin";
            string txt = "Admin Request";
            MessageBoxButton button = MessageBoxButton.YesNoCancel;
            MessageBoxResult result = MessageBox.Show(msgtext, txt, button);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    break;
                case MessageBoxResult.No:
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }
    }
}