// This class is automatically started on the server. Clients connect to the server via this class and can open new sessions (game rounds).
// Each session creates a new SessionID and then an instance of the class Game.  
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using MDC.Gamedata;
using MDC.Gamedata.PlayerType;
using MDC.Server;

public class MasterServer
{
    static int PORT_NO;
    static string SERVER_IP;
    public static Boolean Shutdown { get; set; }

    static private Dictionary<string, Game> _games = new Dictionary<string, Game>(); //sessionID, Game
    static private Dictionary<string, TcpClient> _clients = new Dictionary<string, TcpClient>(); //clientID, TcpClient
    static private CommandManager scm = new CommandManager();

    /// <summary>
    /// Prepare the server for the connection of new clients.
    /// </summary>
    public static void StartServer()
    {
        ReadConfig();
        // CommandManager scm = new CommandManager();

        //---listen at the specified IP and port no.---
        IPAddress localAdd = IPAddress.Any; //Parse(SERVER_IP);
        TcpListener listener = new TcpListener(localAdd, PORT_NO);

        Console.WriteLine("IP: " + SERVER_IP);
        Console.WriteLine("Port: " + PORT_NO);
        Console.WriteLine("Listening...");
        listener.Start();

        Shutdown = false; //Wird nicht klappen, da der Server in listerner.AccepptTcpClient() hängt

        while (Shutdown == false)
        {
            TcpClient tcpClient = listener.AcceptTcpClient();

            Thread clientThread = new Thread(new ThreadStart(() => ClientInteraction(tcpClient, new CommandManager())));
            clientThread.Start();
        }

        Console.WriteLine("Shuting down Server...");

        listener.Stop();
    }

    /// <summary>
    /// Read the configuration from the config-file and store the value in the corresponding variables.
    /// </summary>
    private static void ReadConfig()
    {
        var data = new Dictionary<string, string>();
        foreach (var row in File.ReadAllLines("game.config"))
            data.Add(row.Split('=')[0], string.Join("=", row.Split('=').Skip(1).ToArray()));

        SERVER_IP = data["ServerIP"];
        PORT_NO = Convert.ToInt32(data["PortNo"]);
    }

    /// <summary>
    /// [Only called by received commands] Creates a new game session and saves it in the dictionary.
    /// </summary>
    /// <param name="client_ID">ID of the executing client</param>
    public static void CreateNewGame(string client_ID)
    {
        string session_ID = GenerateID();
        _games.Add(session_ID, new Game(session_ID, _clients.GetValueOrDefault(client_ID)));

        SendCommandToClient(_clients.GetValueOrDefault(client_ID), new CommandFeedbackActionExecutedSuccessfully(client_ID));
        SendStringToClient(_clients.GetValueOrDefault(client_ID), session_ID);
    }

    /// <summary>
    /// [Only called by received commands] Connects a client to an existing game session.
    /// </summary>
    /// <param name="client_ID">ID of the executing client</param>
    /// <param name="session_ID">ID of the game the client wants to connect to</param>
    public static void ConnectToGame(string client_ID, string session_ID)
    {
        Console.WriteLine("Connecting to " + session_ID + " with client " + client_ID);
        _games.GetValueOrDefault(session_ID).AddClientToGame(_clients.GetValueOrDefault(client_ID));

        //TODO: Auf Fehler prüfen, z.B. bei falscher ID
        SendCommandToClient(_clients.GetValueOrDefault(client_ID), new CommandFeedbackActionExecutedSuccessfully(client_ID));
        SendStringToClient(_clients.GetValueOrDefault(client_ID), session_ID);
    }

    /// <summary>
    /// Forwards a player object created by the command to the correct game session.
    /// </summary>
    /// <param name="client_ID">ID of the executing client</param>
    /// <param name="session_ID">ID of the game the client wants to connect to</param>
    /// <param name="player">The player object to be forwarded</param>
    public static void CreateNewPlayerForSession(string client_ID, string session_ID, Player player)
    {
        _games.GetValueOrDefault(session_ID).AddPlayerToGame(client_ID, player);
        SendCommandToClient(_clients.GetValueOrDefault(client_ID), new CommandFeedbackActionExecutedSuccessfully(client_ID));
    }

    /// <summary>
    /// [Only called by received commands] Starts the game round. Executable only when all players are connected.
    /// </summary>
    /// <param name="session_ID">ID of the game session to be started</param>
    public static void StartGame(string session_ID)
    {
        _games.GetValueOrDefault(session_ID).StartGame();
    }

    /// <summary>
    /// Thread implementation for clients. Waits for commands from client. 
    /// </summary>
    /// <param name="client">The TcpClient which belongs to this thread</param>
    /// <param name="cm">The server-wide CommandManager</param>
    private static void ClientInteraction(TcpClient client, CommandManager cm)
    {
        string client_ID = GenerateID();
        _clients.Add(client_ID, client);

        //---write back the client ID to the client---
        SendStringToClient(client, client_ID);

        while (client.Connected)
        {
            Console.WriteLine("\n ------------------------ \n Waiting for Command... \n ------------------------");
            Command leCommand = ReceiveCommandFromClient(client);
            if (leCommand != null)
            {
                cm.AddCommand(leCommand);
                cm.ProcessPendingTransactions();
            }
            else
            {
                client.Close();
            }
        }
        client.Close();
    }

    /// <summary>
    /// Generate an unique ID
    /// </summary>
    /// <returns>Returns the unique ID</returns>
    private static string GenerateID()
    {
        return Guid.NewGuid().ToString("N");
    }

    /// <summary>
    /// Receive string from client
    /// </summary>
    /// <param name="client">TcpClient from which data is to be received.</param>
    /// <returns>Returns the received string</returns>
    private static string ReceiveStringFromClient(TcpClient client)
    {
        NetworkStream nwStream = client.GetStream();
        byte[] bytesToRead = new byte[client.ReceiveBufferSize];
        // Console.WriteLine("TYPE: " + bytesToRead.GetType());
        int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);

        return Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
    }

    /// <summary>
    /// Send string to client
    /// </summary>
    /// <param name="client">TcpClient to which data is to be sent.</param>
    /// <param name="data">String you want to send</param>
    private static void SendStringToClient(TcpClient client, string data)
    {
        NetworkStream nwStream = client.GetStream();
        byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(data);
        nwStream.Write(bytesToSend, 0, bytesToSend.Length);
    }

    /// <summary>
    /// Receive Command from client
    /// </summary>
    /// <param name="client">TcpClient from which data is to be received.</param>
    /// <returns>Returns the received Command</returns>
    private static Command ReceiveCommandFromClient(TcpClient client)
    {
        NetworkStream nwStream = client.GetStream();
        MemoryStream dataStream = new MemoryStream();
        IFormatter formatter = new BinaryFormatter();

        byte[] bytesToRead = new byte[client.ReceiveBufferSize];
        int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);

        dataStream.Write(bytesToRead, 0, bytesToRead.Length);
        dataStream.Seek(0, SeekOrigin.Begin);

        try
        {
            var obj = formatter.Deserialize(dataStream);
            Console.WriteLine(obj.GetType());
            if (obj.GetType().IsSubclassOf(typeof(Command)))
            {
                return (Command)obj;
            }
        }
        catch (System.Runtime.Serialization.SerializationException e)
        {
            Console.WriteLine(e.Message);
        }

        return null;
    }

    /// <summary>
    /// Send Command to client
    /// </summary>
    /// <param name="server">TcpClient to which data is to be sent.</param>
    /// <param name="command">Command you want to send</param>
    private static void SendCommandToClient(TcpClient client, Command command)
    {
        NetworkStream nwStream = client.GetStream();
        IFormatter formatter = new BinaryFormatter();
        formatter.Serialize(nwStream, command);
    }
}