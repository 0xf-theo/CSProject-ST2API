using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Chat.Common.Packets
{
    [Serializable]
    public class RegisterAuthPacket : Packet
    {

        public string Username { get; set; }
        public string Password { get; set; }

        public int Id => 3;
    }
}
