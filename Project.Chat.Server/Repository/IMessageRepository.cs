using Project.Chat.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Chat.Server.Repository
{
    public interface IMessageRepository
    {
        List<Message> ByTopic(Topic topic);

        void Save(Message message);

    }
}
