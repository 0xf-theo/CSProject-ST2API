using Project.Chat.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Chat.Server.Repository
{
    public interface ITopicRepository
    {
        List<Topic> All();

        Topic ByName(string name);

        void Save(Topic topic);
    }
}
