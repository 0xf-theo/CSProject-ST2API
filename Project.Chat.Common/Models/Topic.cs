using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Chat.Common.Models
{
    [Serializable]
    public class Topic
    {

        public Guid Id { get; set; }
        public string Name { get; set; }

    }
}
