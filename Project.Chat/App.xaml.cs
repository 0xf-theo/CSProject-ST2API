using Project.Chat.modals;
using Project.Chat.viewModels;
using System.Configuration;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows;

namespace Project.Chat
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    ///
    public partial class App : Application
    {
        private static SocketClient _socketClient;
        public static SocketClient SocketClient { get { return _socketClient; } }

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _socketClient = new SocketClient("localhost", 12345);
            bool isConnected = await _socketClient.ConnectAsync();

            if (!isConnected)
            {
                ShowErrorModal("Failed to connect to the server at localhost:12345.");
            }
            else
            {
                // Continue with application startup
                LoginRegisterWindow loginRegisterWindow = new LoginRegisterWindow();
                loginRegisterWindow.Show();

                _socketClient.OnConnected += (s, e) => {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();

                    loginRegisterWindow.Close();
                };
            }
        }

        private void ShowErrorModal(string errorMessage)
        {
            ErrorModal errorModal = new ErrorModal(errorMessage);
            errorModal.ShowDialog();
            Shutdown(); 
        }
    }
}
