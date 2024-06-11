using Project.Chat.Common.Models;

namespace Project.Chat.Common.Packets
{
    [Serializable]
    public class SendMessagePacket : Packet
    {
        public Topic Topic { get; set; }

        public string Content { get; set; }

        public int Id => 7;
    }
}
