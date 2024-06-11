using Project.Chat.Common.Model;
using Project.Chat.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Project.Chat.Server.Repository
{
    public class FileBasedTopicRepository : ITopicRepository
    {

        private readonly string filePath;
        private List<Topic> topics;

        public FileBasedTopicRepository(string filePath)
        {
            this.filePath = filePath;
            LoadTopics();
        }

        private void LoadTopics()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                topics = JsonSerializer.Deserialize<List<Topic>>(json);
            }
            else
            {
                topics = new List<Topic>();
            }
        }

        private void SaveTopics()
        {
            string json = JsonSerializer.Serialize(topics, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public List<Topic> All()
        {
            return topics;
        }

        public void Save(Topic topic)
        {
            topics.Add(topic);
            SaveTopics();
        }

        public Topic ByName(string name)
        {
            return topics.FirstOrDefault(u => u.Name.Equals(name));
        }
    }
}
