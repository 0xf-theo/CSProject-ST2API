using Project.Chat.Common.Packets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Project.Chat.Common;
using Project.Chat.Server.Repository;
using Project.Chat.Common.Model;
using Project.Chat.Server.Util;
using Project.Chat.Common.Models;
using System.Text.Json;

namespace Project.Chat.Server
{
    public class Client
    {
        private TcpClient tcpClient;
        private NetworkStream stream;
        public string clientId { get; private set; }

        private User user;
        private IUserRepository userRepository;
        private ITopicRepository topicRepository;
        private IMessageRepository messageRepository;

        private bool IsConnected => user != null;

        public Client(TcpClient client,
            IUserRepository userRepository, 
            ITopicRepository topicRepository,
            IMessageRepository messageRepository)
        {
            tcpClient = client;
            stream = tcpClient.GetStream();
            clientId = Guid.NewGuid().ToString();
            this.userRepository = userRepository;
            this.topicRepository = topicRepository;
            this.messageRepository = messageRepository;
        }

        public void SendPacket<T>(T packet) where T : Packet
        {
            PacketWrapper wrapper = new PacketWrapper
            {
                PacketId = packet.Id,
                Data = PacketUtils.Serialize(packet)
            };
            byte[] data = PacketUtils.Serialize(wrapper);
            stream.Write(data, 0, data.Length);
        }

        public void HandlePacket(PacketWrapper packet)
        {
            switch (packet.PacketId)
            {
                case 1:
                    LoginAuthPacket loginAuth = PacketUtils.Deserialize<LoginAuthPacket>(packet.Data);
                    HandleLoginAuth(loginAuth);
                    break;
                case 3:
                    RegisterAuthPacket registerAuthPacket = PacketUtils.Deserialize<RegisterAuthPacket>(packet.Data);
                    HandleRegisterAuth(registerAuthPacket);
                    break;
                case 5:
                    CreateTopicPacket createTopic = PacketUtils.Deserialize<CreateTopicPacket>(packet.Data);
                    HandleCreateTopic(createTopic);
                    break;
                case 7:
                    SendMessagePacket sentMessage = PacketUtils.Deserialize<SendMessagePacket>(packet.Data);
                    HandleSendMessage(sentMessage);
                    break;
                case 8:
                    ShowTopicPacket showTopic = PacketUtils.Deserialize<ShowTopicPacket>(packet.Data);
                    HandleShowTopic(showTopic);
                    break;
            }
        }

        private async void TrackToken(String symbol, Topic topic)
        {
            string url = "https://tokenstracker.iambluedev.workers.dev/?symbol=" + symbol;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    
                    CryptoData cryptoData = JsonSerializer.Deserialize<CryptoData>(responseBody);

                    Console.WriteLine(responseBody);

                    SystemNotify($"Name: {cryptoData.name}" +
                       $"\nSymbol: {cryptoData.symbo}" +
                       $"\nCirculating Supply: {cryptoData.circulating_supply}" +
                       $"\nTotal Supply: {cryptoData.total_supply}" +
                       $"\nPrice: {cryptoData.price}"
                       , topic);
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Request error: {e.Message}");
                    SystemNotify($"Le token {symbol} n'existe pas", topic);
                }
            }
        }

        private void SystemNotify(string content, Topic topic)
        {
            var message = new Message()
            {
                Id = Guid.NewGuid(),
                Topic = topic,
                Content = content,
                Sender = AppServer.SystemUser,
                Time = DateTime.Now
            };

            messageRepository.Save(message);

            AppServer.BroadcastPacket(new TopicHistoryPacket()
            {
                Topic = topic,
                Messages = [message]
            });
        }

        private void HandleSendMessage(SendMessagePacket packet)
        {
            Console.WriteLine("[" + packet.Topic.Name + "] " + user.Username + ":" + packet.Content);

            if(packet.Content.StartsWith("/track")) {
                var parts = packet.Content.Split(" ");

                if(parts.Length == 2)
                {
                    var token = parts[1];
                    TrackToken(token.Trim(), packet.Topic);
                } else
                {
                    SystemNotify("La commande entrée n'est pas correcte", packet.Topic);
                }
            }

            var message = new Message()
            {
                Id = Guid.NewGuid(),
                Topic = packet.Topic,
                Content = packet.Content,
                Sender = user,
                Time = DateTime.Now
            };

            messageRepository.Save(message);

            AppServer.BroadcastPacket(new TopicHistoryPacket()
            {
                Topic = packet.Topic,
                Messages = [message]
            });
        }

        private void HandleShowTopic(ShowTopicPacket packet)
        {
            var messages = messageRepository.ByTopic(packet.Topic);
           

            SendPacket(new TopicHistoryPacket()
            {
                Topic = packet.Topic,
                Messages = messages
            });
        }

        private void HandleLoginAuth(LoginAuthPacket packet)
        {
            Console.WriteLine($"Login attempt: {packet.Username}");

            var user = userRepository.ByUsername(packet.Username);

            if (user != null)
            {
                bool isPasswordCorrect = PasswordChecker.CheckPassword(user.Password, packet.Password);
                Console.WriteLine("Mot de passe correct: " + isPasswordCorrect);

                if (isPasswordCorrect)
                {
                    this.user = user;

                    user.Password = null;

                    SendPacket(new LoginSuccessPacket()
                    {
                        User = user,
                    });

                    SendPacket(new TopicsListPacket()
                    {
                        Topics = topicRepository.All()
                    });
                } else
                {
                    SendPacket(new ErrorPacket()
                    {
                        Message = "Mot de passe incorrect"
                    });
                }
            }
            else
            {
                Console.WriteLine("Utilisateur non trouvé.");

                SendPacket(new ErrorPacket()
                {
                    Message = "Utilisateur n'existe pas"
                });
            }
        }

        private void HandleRegisterAuth(RegisterAuthPacket registerAuthPacket)
        {
            Console.WriteLine("Register " + registerAuthPacket.Username);

            User user = userRepository.ByUsername(registerAuthPacket.Username);
            if(user != null)
            {
                Console.WriteLine("Utilisateur existe déjà.");

                SendPacket(new ErrorPacket()
                {
                    Message = "Username déjà utilisé"
                });
            } else
            {
                User newUser = new User
                {
                    Id = Guid.NewGuid(),
                    Username = registerAuthPacket.Username,
                    Password = PasswordChecker.HashPassword(registerAuthPacket.Password),
                    AvatarUrl = "https://www.gravatar.com/avatar/?d=identicon"
                };

                userRepository.Save(newUser);

                Console.WriteLine("Utilisateur enregistré avec id " + newUser.Id);

                SendPacket(new ErrorPacket()
                {
                    Message = "Vous pouvez désormais vous connecter !"
                });
            } 
        }

        private void HandleCreateTopic(CreateTopicPacket packet)
        {
            Console.WriteLine("Create topic " + packet.Name);

            Topic topic = topicRepository.ByName(packet.Name);
            if (topic != null)
            {
                Console.WriteLine("Topic existe déjà.");

                SendPacket(new ErrorPacket()
                {
                    Message = "Topic déjà présent"
                });
            }
            else
            {
                Topic newTopic = new Topic
                {
                    Id = Guid.NewGuid(),
                    Name = packet.Name,
                };

                topicRepository.Save(newTopic);

                Console.WriteLine("Topic enregistré avec id " + newTopic.Id);

                AppServer.BroadcastPacket(new TopicsListPacket()
                {
                    Topics = topicRepository.All()
                });
            }
        }
    }

    [Serializable]
    public class CryptoData
    {
        public string name { get; set; }
        public string symbo { get; set; }
        public decimal circulating_supply { get; set; }
        public decimal total_supply { get; set; }
        public decimal price { get; set; }
    }
}
