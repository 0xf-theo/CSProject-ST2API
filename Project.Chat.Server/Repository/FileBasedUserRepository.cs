using Project.Chat.Common.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Project.Chat.Server.Repository
{
    public class FileBasedUserRepository : IUserRepository
    {
        private readonly string filePath;
        private List<User> users;

        public FileBasedUserRepository(string filePath)
        {
            this.filePath = filePath;
            LoadUsers();
        }

        private void LoadUsers()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                users = JsonSerializer.Deserialize<List<User>>(json);
            }
            else
            {
                users = new List<User>();
            }
        }

        private void SaveUsers()
        {
            string json = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, json);
        }

        public User ByUsername(string username)
        {
            return users.FirstOrDefault(u => u.Username.Equals(username));
        }

        public void Save(User user)
        {
            users.Add(user);
            SaveUsers();
        }
    }
}
