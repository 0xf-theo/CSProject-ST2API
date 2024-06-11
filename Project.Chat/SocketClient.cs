using Project.Chat.Common.Packets;
using Project.Chat.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Project.Chat.modals;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using Project.Chat.Common.Model;
using Project.Chat.Common.Models;
using Project.Chat.viewModels;
using System.Text.Json;

namespace Project.Chat
{
    public class SocketClient
    {
        private readonly string _server;
        private readonly int _port;
        private TcpClient _client;
        private NetworkStream _stream;
        private Thread _clientThread;
        private User _user;

        public event EventHandler OnConnected;

        public User User { get { return _user; } }

        public SocketClient(string server, int port)
        {
            _server = server;
            _port = port;
        }

        public async Task<bool> ConnectAsync()
        {
            try
            {
                _client = new TcpClient();
                await _client.ConnectAsync(_server, _port);
                _stream = _client.GetStream();

                 _clientThread = new Thread(() =>
                {
                    byte[] buffer = new byte[4096*10]; // On devrait mieux gerer la taille des packet qu'on recoit en ajoutant dans lentete le nb d'octet
                    int bytesRead;

                    try
                    {
                        while ((bytesRead = _stream.Read(buffer, 0, buffer.Length)) != 0)
                        {
                            byte[] data = new byte[bytesRead];
                            Array.Copy(buffer, data, bytesRead);
                            PacketWrapper packet = PacketUtils.Deserialize<PacketWrapper>(data);
                            Console.WriteLine($"Reçu paquet avec ID: {packet.PacketId}");

                            HandlePacket(packet);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Erreur: {e.Message}");
                    }
                    finally
                    {
                        _stream.Close();
                        _client.Close();
                        Console.WriteLine("Client déconnecté...");
                    }
                });

                _clientThread.Start();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void Disconnect()
        {
            _client?.Close();
        }

        public void SendPacket<T>(T packet) where T : Packet
        {
            PacketWrapper wrapper = new PacketWrapper
            {
                PacketId = packet.Id,
                Data = PacketUtils.Serialize(packet)
            };
            byte[] data = PacketUtils.Serialize(wrapper);
            try
            {
                _stream.Write(data, 0, data.Length);
            } catch (Exception e) {}
            
        }

        public void HandlePacket(PacketWrapper packet)
        {
            try
            {
                switch (packet.PacketId)
                {
                    case 2:
                        ErrorPacket error = PacketUtils.Deserialize<ErrorPacket>(packet.Data);
                        HandleError(error);
                        break;
                    case 4:
                        LoginSuccessPacket loginSuccess = PacketUtils.Deserialize<LoginSuccessPacket>(packet.Data);
                        HandleLogin(loginSuccess);
                        break;
                    case 6:
                        TopicsListPacket topicsList = PacketUtils.Deserialize<TopicsListPacket>(packet.Data);
                        HandleTopicsList(topicsList);
                        break;
                    case 9:
                        TopicHistoryPacket topicHistory = PacketUtils.Deserialize<TopicHistoryPacket>(packet.Data);
                        HandleTopicHistory(topicHistory);
                        break;
                }
            } catch (JsonException e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void HandleTopicHistory(TopicHistoryPacket topicHistory)
        {
            var tmp = new List<Message>(Store.Instance.Messages);

            topicHistory.Messages.ForEach(m =>
            {
                if(tmp.Find(m2 => m.Id.Equals(m2.Id)) == null)
                {
                    m.IsSentByCurrentUser = m.Sender.Id.Equals(_user.Id);
                    tmp.Add(m);
                }
            });

            Store.Instance.Messages = tmp;
        }

        private void HandleError(ErrorPacket packet)
        {
            // Doit aller dans le thread UI
            Application.Current.Dispatcher.Invoke(new Action(() => {
                ErrorModal errorModal = new ErrorModal(packet.Message);
                errorModal.ShowDialog();
            }));
        }

        private void HandleLogin(LoginSuccessPacket loginSuccess)
        {
            _user = loginSuccess.User;
            Store.Instance.ConnectedUser = _user;

            Application.Current.Dispatcher.Invoke(new Action(() => {
                OnConnected.Invoke(this, new EventArgs());
            }));
        }

        public void HandleTopicsList(TopicsListPacket topicsList)
        {
            if(topicsList.Topics is not null)
                Store.Instance.Topics = topicsList.Topics; 
        }
    }
}
