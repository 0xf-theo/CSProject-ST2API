using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Chat.Common.Packets
{
    [Serializable]
    public class CreateTopicPacket : Packet
    {

        public string Name { get; set; }

        public int Id => 5;
    }
}
