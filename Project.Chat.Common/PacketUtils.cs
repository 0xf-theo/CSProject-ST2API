using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Project.Chat.Common
{
    public static class PacketUtils
    {
        public static byte[] Serialize<T>(T obj)
        {
            if (obj == null) return null;
            Console.WriteLine("Sending: " + JsonSerializer.Serialize(obj));
            return JsonSerializer.SerializeToUtf8Bytes(obj);
        }

        public static T Deserialize<T>(byte[] data)
        {
            if (data == null) return default(T);
            //Console.WriteLine("Received: " + Encoding.Default.GetString(data));
            return JsonSerializer.Deserialize<T>(data);
        }
    }
}
