using Project.Chat.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Chat.Common.Packets
{
    [Serializable]
    public class LoginSuccessPacket : Packet
    {
        public User User { get; set; }

        public int Id => 4;
    }
}
