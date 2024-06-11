using Project.Chat.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Chat.Common.Packets
{
    [Serializable]
    public class TopicsListPacket : Packet
    {

        public List<Topic> Topics {  get; set; }

        public int Id => 6;
    }
}
