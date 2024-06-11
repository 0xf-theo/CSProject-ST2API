using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Chat.Common.Packets
{
    public interface Packet
    {
        public int Id { get; }
    }
}
