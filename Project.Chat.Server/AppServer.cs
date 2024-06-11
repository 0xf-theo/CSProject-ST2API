using Project.Chat.Common.Packets;
using Project.Chat.Common;
using System.Net;
using System.Net.Sockets;
using Project.Chat.Server;
using Project.Chat.Server.Repository;
using Project.Chat.Common.Model;

public class AppServer
{

    private static List<Client> connectedClients = new List<Client>();
    private static IUserRepository userRepository;
    private static ITopicRepository topicRepository;
    private static IMessageRepository messageRepository;

    public static User SystemUser = new User()
    {
        Username = "System",
        AvatarUrl = "https://gravatar.com/avatar/system",
        Id = Guid.NewGuid()
    };

    static void Main(string[] args)
    {
        int port = 12345;
        IPAddress ipAddress = IPAddress.Any;
        TcpListener listener = new TcpListener(ipAddress, port);

        userRepository = new FileBasedUserRepository("./users.json");
        topicRepository = new FileBasedTopicRepository("./topics.json");
        messageRepository = new FileBasedMessageRepository("./messages.json");

        listener.Start();
        Console.WriteLine("Serveur de socket démarré...");

        while (true)
        {
            // Accepter une nouvelle connexion
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("Client connecté...");

            // Créer un nouveau thread pour gérer la connexion
            Thread clientThread = new Thread(() => HandleClient(client));
            clientThread.Start();
        }
    }

    public static void AddClient(Client clientHandler)
    {
        lock (connectedClients)
        {
            connectedClients.Add(clientHandler);
        }
    }

    public static void RemoveClient(Client clientHandler)
    {
        lock (connectedClients)
        {
            connectedClients.Remove(clientHandler);
        }
    }

    public static List<Client> GetConnectedClients()
    {
        lock (connectedClients)
        {
            return new List<Client>(connectedClients);
        }
    }

    static void HandleClient(TcpClient tpcClient)
    {
        var client = new Client(tpcClient, userRepository, topicRepository, messageRepository);
        AddClient(client);
        NetworkStream stream = tpcClient.GetStream();
        byte[] buffer = new byte[4096 * 10];
        int bytesRead;

        try
        {
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                byte[] data = new byte[bytesRead];
                Array.Copy(buffer, data, bytesRead);
                PacketWrapper packet = PacketUtils.Deserialize<PacketWrapper>(data);
                Console.WriteLine($"Reçu paquet avec ID: {packet.PacketId}");

                client.HandlePacket(packet);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Erreur: {e.Message}");
        }
        finally
        {
            stream.Close();
            tpcClient.Close();
            RemoveClient(client);
            Console.WriteLine("Client déconnecté...");
        }
    }
    public static void BroadcastPacket<T>(T packet) where T : Packet
    {
        connectedClients.ForEach(client => { 
            client.SendPacket<T>(packet);
        });
    }
}