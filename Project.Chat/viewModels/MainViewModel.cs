using Project.Chat.Common.Model;
using Project.Chat.Common.Models;
using Project.Chat.Common.Packets;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Project.Chat.viewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private User _user;

        private bool _isDropdownOpen;
        private ObservableCollection<Topic> _channels;
        private ObservableCollection<Message> _messages;
        private Topic _selectedChannel;
        private string _newMessage;

        public User User
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged();
            }
        }

        public bool IsDropdownOpen
        {
            get => _isDropdownOpen;
            set
            {
                _isDropdownOpen = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Topic> Channels
        {
            get => _channels;
            set
            {
                _channels = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Message> Messages
        {
            get => _messages;
            set
            {
                _messages = value;
                OnPropertyChanged();
            }
        }

        public Topic SelectedChannel
        {
            get => _selectedChannel;
            set
            {
                _selectedChannel = value;
                OnPropertyChanged();
                LoadMessagesForChannel(_selectedChannel);
            }
        }

        public string NewMessage
        {
            get => _newMessage;
            set
            {
                _newMessage = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void Logout()
        {
            // Logic for logging out
        }

        private void LoadMessagesForChannel(Topic channel)
        {
            Messages?.Clear();

            App.SocketClient.SendPacket(new ShowTopicPacket()
            {
                Topic = _selectedChannel,
            });
        }

        public void SendMessage()
        {

            if (!string.IsNullOrWhiteSpace(NewMessage))
            {
                App.SocketClient.SendPacket(new SendMessagePacket()
                {
                    Topic = _selectedChannel,
                    Content = NewMessage
                });

                NewMessage = string.Empty;
            }
        }
    }
}
