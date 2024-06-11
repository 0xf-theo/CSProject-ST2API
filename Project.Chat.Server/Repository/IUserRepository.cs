using Project.Chat.Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Chat.Server.Repository
{
    public interface IUserRepository
    {
        User ByUsername(string username);

        void Save(User user);
    }
}
