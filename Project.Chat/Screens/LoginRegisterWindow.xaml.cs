using Project.Chat.Common.Packets;
using Project.Chat.viewModels;
using System.Windows;
using System.Windows.Controls;

namespace Project.Chat
{
    /// <summary>
    /// Logique d'interaction pour LoginRegisterWindow.xaml
    /// </summary>
    public partial class LoginRegisterWindow : Window
    {
        public LoginRegisterWindow()
        {
            InitializeComponent();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginRegisterViewModel viewModel && sender is PasswordBox passwordBox)
            {
                viewModel.Password = passwordBox.Password;
            }
        }

        private void NewPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginRegisterViewModel viewModel && sender is PasswordBox passwordBox)
            {
                viewModel.NewPassword = passwordBox.Password;
            }
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginRegisterViewModel viewModel && sender is PasswordBox passwordBox)
            {
                viewModel.ConfirmPassword = passwordBox.Password;
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as LoginRegisterViewModel;
            if (viewModel != null)
            {
                Console.WriteLine($"Login - Username: {viewModel.Username}, Password: {viewModel.Password}");
                App.SocketClient.SendPacket(new LoginAuthPacket() { 
                    Username = viewModel.Username,
                    Password = viewModel.Password
                });
            } else {
                Console.WriteLine("no data");
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as LoginRegisterViewModel;
            if (viewModel != null)
            {
                Console.WriteLine($"Register - Username: {viewModel.NewUsername}, Password: {viewModel.NewPassword}, Confirm Password: {viewModel.ConfirmPassword}");

                App.SocketClient.SendPacket(new RegisterAuthPacket()
                {
                    Username = viewModel.NewUsername,
                    Password = viewModel.NewPassword
                });
            }
        }
    }
}
