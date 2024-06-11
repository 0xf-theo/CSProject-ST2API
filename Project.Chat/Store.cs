using Project.Chat.Common.Model;
using Project.Chat.Common.Models;
using Project.Chat.Common.Packets;

namespace Project.Chat
{
    public class Store
    {
        public static Store Instance { get; set; } = new Store();

        public event EventHandler OnUpdate;

        private List<Topic> _topics = new List<Topic>();
        private User _user;
        private List<Message> _messages = new List<Message>();

        public User ConnectedUser
        {
            get => _user;
            set
            {
                _user = value;
                OnUpdate?.Invoke(this, new EventArgs()); 
            }
        }

        public List<Topic> Topics
        {
            get => _topics;
            set
            {
                _topics = value;
                OnUpdate?.Invoke(this, new EventArgs());
            }
        }

        public List<Message> Messages
        {
            get => _messages;
            set
            {
                _messages = value;
                OnUpdate?.Invoke(this, new EventArgs());
            }
        }
    }
}
