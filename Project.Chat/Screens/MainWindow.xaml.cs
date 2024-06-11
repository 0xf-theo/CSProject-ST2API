using Project.Chat.Common.Model;
using Project.Chat.Common.Models;
using Project.Chat.modals;
using Project.Chat.viewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Project.Chat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel;
        private string _selectedTab = "Topics";

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = DataContext as MainViewModel;

            _viewModel.User = Store.Instance.ConnectedUser;
            _viewModel.Channels = [];
            _viewModel.Messages = [];

            Store.Instance.Topics.ForEach(topic => { _viewModel.Channels.Add(topic); });

            Store.Instance.OnUpdate += (e, e1) =>
            {
                _viewModel.User = Store.Instance.ConnectedUser;
                _viewModel.Channels = [];
                _viewModel.Messages = [];

                Store.Instance.Topics.ForEach(topic => { _viewModel.Channels.Add(topic); });

                if(_viewModel.SelectedChannel is not null)
                {
                    var messages = Store.Instance.Messages
                        .FindAll(m => m.Topic.Id.Equals(_viewModel.SelectedChannel.Id));

                    _viewModel.Messages = new System.Collections.ObjectModel.ObservableCollection<Message>(messages);
                }
            };
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                _selectedTab = (e.AddedItems[0] as TabItem)?.Header.ToString();

                if (_selectedTab == "Topics")
                {
                    CreateButton.Content = "Create Topic";
                }
            }
        }

        private void TopicsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ListBox listBox && listBox.SelectedItem is Topic selectedChannel)
            {
                _viewModel.SelectedChannel = selectedChannel;

                var messages = Store.Instance.Messages
                         .FindAll(m => m.Topic.Id.Equals(_viewModel.SelectedChannel.Id));

                _viewModel.Messages = new System.Collections.ObjectModel.ObservableCollection<Message>(messages);
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedTab == "Topics")
            {
                CreateTopicModal createTopicModal = new CreateTopicModal();
                createTopicModal.ShowDialog();
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SendMessage();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is ComboBoxItem item && item.Content.ToString() == "Déconnexion")
            {
                _viewModel.Logout();
            }
        }
    }
}