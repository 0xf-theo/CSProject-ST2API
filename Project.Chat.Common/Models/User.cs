using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Project.Chat.Common.Model
{
    [Serializable]
    public class User
    {

        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string AvatarUrl { get; set; }

    }
}
