using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Chat.Common.Packets
{
    [Serializable]
    public class PacketWrapper
    {
        public int PacketId { get; set; }
        public byte[] Data { get; set; }
    }
}
