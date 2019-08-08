//TODO: Implement this class
// This class is automatically started on the server. Clients connect to the server via this class and can open new sessions (game rounds).
// Each session creates a new SessionID and then an instance of the class Game.  
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using MDC.Client;
using MDC.Gamedata;
using MDC.Server;

class MasterServer
{
    const int PORT_NO = 5000;
    const string SERVER_IP = "127.0.0.1";
    public static Boolean Shutdown { get; set; }

    static private Dictionary<string, Game> _games = new Dictionary<string, Game>(); //sessionID, Game
    static private Dictionary<string, TcpClient> _clients = new Dictionary<string, TcpClient>(); //clientID, TcpClient
    static private CommandManager scm = new CommandManager();

    public static void StartServer()
    {

        // CommandManager scm = new CommandManager();

        //---listen at the specified IP and port no.---
        IPAddress localAdd = IPAddress.Parse(SERVER_IP);
        TcpListener listener = new TcpListener(localAdd, PORT_NO);

        Console.WriteLine("IP: " + SERVER_IP);
        Console.WriteLine("Port: " + PORT_NO);
        Console.WriteLine("Listening...");
        listener.Start();

        Shutdown = false; //Wird nicht klappen, da der Server in listerner.AccepptTcpClient() hängt

        while (Shutdown == false)
        {
            TcpClient tcpClient = listener.AcceptTcpClient();

            //TODO: Hier evtl. direkt Command von Client empfangen und auswerten ob Neues Spiel starten oder mit Session verbinden. 
            //      Erst dann neuen Thread erzeugen und diesen evtl an Game weiterreichen???
            //      Nee das ist doch kacke: Da Clients sich dann für jedes neues Spiel reconnecten müssen
            Thread clientThread = new Thread(new ThreadStart(() => ClientInteraction(tcpClient, new CommandManager())));
            clientThread.Start();
        }

        Console.WriteLine("Shuting down Server");

        listener.Stop();
    }

    public static void CreateNewGame(string clientID)
    {
        string sessionID = GenerateID();

        //TODO: Ausgabe entfernen
        Console.WriteLine("Session ID: " + sessionID + "; Client ID: " + clientID);
        SendStringToClient(_clients.GetValueOrDefault(clientID), sessionID);

        // _games.Add(sessionID, new Game(sessionID, _connectedClients.GetValueOrDefault(clientID)));
        _games.Add(sessionID, new Game(sessionID, _clients.GetValueOrDefault(clientID)));

        // GetInputFromClient(clientID, sessionID);
    }

    public static void ConnectToGame(string clientID, string sessionID, string playerName)
    {
        Console.WriteLine("Connecting to " + sessionID + " with client " + clientID + " and playerName " + playerName);
        _games.GetValueOrDefault(sessionID).AddClientToGame(_clients.GetValueOrDefault(clientID), playerName);
        // Game currentGame = _games.GetValueOrDefault(sessionID);

        // if (_clients.ContainsKey(clientID))
        // {
        //     TcpClient currentClient = _clients.GetValueOrDefault(clientID);
        //     currentGame.AddClientToGame(currentClient, playerName);
        // }
        // else
        // {
        //     Console.WriteLine("Object not found");
        // }

        // GetInputFromClient(clientID, sessionID);
    }

    public static void StartGame(string sessionID)
    {
        _games.GetValueOrDefault(sessionID).StartGame();
        // GetInputFromClient(clientID, sessionID);
    }

    // private static void GetInputFromClient(string clientID, string sessionID)
    // {
    //     ServerCommand command = (ServerCommand) ReceiveCommandFromClient(_clients.GetValueOrDefault(clientID));
    //     command.TargetGame = _games.GetValueOrDefault(sessionID);

    //     scm.AddCommand(command);
    //     scm.ProcessPendingTransactions();
    // }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="client"></param>
    /// <param name="cm"></param>
    private static void ClientInteraction(TcpClient client, CommandManager cm)
    {
        string clientID = GenerateID();
        _clients.Add(clientID, client);

        Console.WriteLine("No. of Clients: " + _clients.Count);
        //---write back the client ID to the client---
        SendStringToClient(client, clientID);

        // while (true)
        // {
        cm.AddCommand(ReceiveCommandFromClient(client));
        cm.ProcessPendingTransactions();
        // }

        // client.Close();
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

        return Encoding.ASCII.GetString(bytesToRead, 0, bytesRead); ;
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
        IFormatter formatter = new BinaryFormatter();

        return (Command)formatter.Deserialize(nwStream); ;
    }
}