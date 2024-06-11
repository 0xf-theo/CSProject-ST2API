using Project.Chat.Common.Models;
using System.Text.Json;

namespace Project.Chat.Server.Repository
{
    public class FileBasedMessageRepository : IMessageRepository
    {

        private readonly string filePath;
        private List<Message> messages;

        public FileBasedMessageRepository(string filePath)
        {
            this.filePath = filePath;
            LoadMessages();
        }

        private void LoadMessages()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                messages = JsonSerializer.Deserialize<List<Message>>(json);
            }
            else
            {
                messages = new List<Message>();
            }
        }

        private void SaveMessages()
        {
            string json = JsonSerializer.Serialize(messages, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public void Save(Message message)
        {
            messages.Add(message);
            SaveMessages();
        }

        public List<Message> ByTopic(Topic topic)
        {
            return messages.FindAll(u => u.Topic.Id.Equals(topic.Id))
                .OrderBy(x => x.Time)
                .ToList();
        }

    }
}
