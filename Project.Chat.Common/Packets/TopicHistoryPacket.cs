using Project.Chat.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Chat.Common.Packets
{
    [Serializable]
    public class TopicHistoryPacket : Packet
    {

        public Topic Topic { get; set; }

        public List<Message>   Messages { get; set; }

        public int Id => 9;
    }
}
