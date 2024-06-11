using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Chat.Common.Packets
{
    [Serializable]
    public class ErrorPacket : Packet
    {
        public string Message { get; set; }

        public int Id => 2;
    }
}
