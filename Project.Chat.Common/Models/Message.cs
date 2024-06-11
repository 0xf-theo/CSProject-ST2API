using Project.Chat.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Project.Chat.Common.Models
{
    [Serializable]
    public class Message
    {
        public Guid Id { get; set; }
        public Topic Topic {  get; set; }
        public User Sender {  get; set; }
        public DateTime Time { get; set; }
        public string Content { get; set; }

        [JsonIgnore]
        public bool IsSentByCurrentUser { get; set; }

        [JsonIgnore]
        public bool IsNotSentByCurrentUser => !IsSentByCurrentUser;

        [JsonIgnore]
        public string FormatedTime => Time.ToString("HH:mm:ss - dd/MM/yyyy");
    }
}
