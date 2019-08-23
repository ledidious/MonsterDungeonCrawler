using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using MDC.Gamedata;
using MDC.Gamedata.PlayerType;

namespace MDC.Server
{
    public class Game
    {
        public int RoundCounter;
        const int MAX_CLIENTS = 4; //Vorher 4, aber da Game vom Hauptclient gehostet wird nur 3

        // Player/Client mapping: the string represents the clientID
        private Dictionary<string, Player> _players = new Dictionary<string, Player>();
        // private Dictionary<string, TcpClient> _clientsOfThisGame = new Dictionary<string, TcpClient>();
        private List<TcpClient> _clientsOfThisGame = new List<TcpClient>();
        private TcpClient _currentClient;
        private Player _currentPlayer;
        private int _lapsRemaining;
        private String _sessionID;
        CommandManager gcm = new CommandManager();

        public Game(String sessionID, TcpClient firstClient)
        {
            _clientsOfThisGame.Add(firstClient);
            this._sessionID = sessionID;
            this._currentClient = firstClient;

            // CommandManager gcm = new CommandManager();

            // while (true)
            // {
            //     gcm.AddCommand(ReceiveCommandFromClient(_currentClient));
            //     gcm.ProcessPendingTransactions();
            // }
        }

        /// <summary>
        /// Adds a player object to the game
        /// </summary>
        /// <param name="client_ID">ID of the owner of the player</param>
        /// <param name="player">The player to add</param>
        public void AddPlayerToGame(string client_ID, Player player)
        {
            player.XPosition = 0; player.YPosition = 1;
            Hero main = (Hero)player;
            Console.WriteLine("Client " + client_ID + " will now add " + main.PlayerName + " to the game...");
            _players.Add(client_ID, main);
        }

        /// <summary>
        /// Creates a new player and adds it to the game
        /// </summary>
        /// <param name="client_ID">ID of the owner of the player</param>
        /// <param name="playerName">Name of the new player</param>
        /// <param name="characterClass">The class of character of the character</param>
        public void AddPlayerToGame(string client_ID, string playerName, string characterClass)
        {
            // switch (characterClass)
            // {

            //     default:
            // }
            // _players.Add()
            throw new NotImplementedException();
        }

        /// <summary>
        /// Adds a TcpClient to the game
        /// </summary>
        /// <param name="client">The TcpClient</param>
        public void AddClientToGame(TcpClient client)
        {
            if (_clientsOfThisGame.Count < MAX_CLIENTS)
            {
                _clientsOfThisGame.Add(client);
            }
            else
            {
                throw new NotImplementedException();
            }

            // throw new NotImplementedException();
        }

        /// <summary>
        /// [Call only when all players are ready] Starts the game round
        /// </summary>
        public void StartGame()
        {
            // gcm.AddCommand(ReceiveCommandFromClient(_currentClient));
            // gcm.ProcessPendingTransactions();

            throw new NotImplementedException();
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
            Console.WriteLine("TYPE: " + bytesToRead.GetType());
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
        private static CommandGame ReceiveCommandFromClient(TcpClient client)
        {
            NetworkStream nwStream = client.GetStream();
            IFormatter formatter = new BinaryFormatter();

            return (CommandGame)formatter.Deserialize(nwStream); ;
        }

        /// <summary>
        /// Ends the turn of a client
        /// </summary>
        /// <returns>TODO: Muss hier etwas zurückgegeben werden, oder muss nur auf den nächsten Client gehorcht werden?</returns>
        public Player NextPlayer()
        {
            // _currentClient = _clientsOfThisGame.FindIndex();
            return null;
        }

        // ############
        // # OLD CODE #
        // ############

        // /// <summary>
        // /// Start a new server, which clients can connect to via a TCP connection
        // /// </summary>
        // public static void StartServer()
        // {

        //     CommandManager cm = new CommandManager();

        //     //---listen at the specified IP and port no.---
        //     IPAddress localAdd = IPAddress.Parse(SERVER_IP);
        //     TcpListener listener = new TcpListener(localAdd, PORT_NO);

        //     Console.WriteLine("IP: " + SERVER_IP);
        //     Console.WriteLine("Listening...");
        //     listener.Start();

        //     int counter = 0;
        //     while (counter < MAX_CLIENTS)
        //     {
        //         counter++;
        //         TcpClient tcpClient = listener.AcceptTcpClient();

        //         Thread clientThread = new Thread(new ThreadStart(() => ClientInteraction(tcpClient, cm)));
        //         clientThread.Start();
        //     }

        //     listener.Stop();
        // }

        /// <summary>
        /// Called when a new Client connects to the server
        /// </summary>
        /// <param name="client">The Tcp Client</param>
        /// <param name="cm">The Command Manager</param>
        /*  private void ClientInteraction(TcpClient client, CommandManager cm)
         {
             string clientID = GenerateID();

             //---Get the playerName from the server and create a new Player---
             string playerName = ReceiveStringFromClient(client);
             // _players.Add(clientID, new Player(playerName, 20));
             //TODO: Make it universal --> Each PlayerType should be addable
             _players.Add(clientID, new Hero("Manfred", 20));

             //---write back the client ID to the client---
             SendStringToClient(client, clientID);

             Console.WriteLine($"Player {_players[clientID].PlayerName} has {_players[clientID].PlayerRemainingMoves} moves left.");

             //---Receive command from client
             GameCommand command = ReceiveCommandFromClient(client);

             //---Get the matching player object from the dictionary and inject it into the command----
             command.TargetPlayer = _players.GetValueOrDefault(command.ClientID);

             cm.AddCommand(command);
             cm.ProcessPendingTransactions();

             Console.WriteLine($"Player {_players[clientID].PlayerName} has {_players[clientID].PlayerRemainingMoves} moves left.");

             client.Close();

         } */

        /*         /// <summary>
                /// Create a new player
                /// </summary>
                /// <param name="playerName">Name of the new player </param>
                /// <returns>Returns the new Player</returns>
                private static Player CreatePlayer(string playerName)
                {
                    return new Player(playerName, 20);
                } */

        // //---incoming client connected---
        // TcpClient client = listener.AcceptTcpClient();
        // NetworkStream nwStream = client.GetStream();

        // //---receive player from client and add it to the dictionary---
        // _players.Add(noc, (Player) formatter.Deserialize(nwStream));

        //     //---write back the client ID to the client---
        //     byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(noc.ToString());
        // nwStream.Write(bytesToSend, 0, bytesToSend.Length);
        //     noc++;

        //     Console.WriteLine($"Player {_players[0].playerName} has {_players[0].playerRemainingMoves} moves left.");

        //     //---Receive command from client
        //     ICommand command = (ICommand)formatter.Deserialize(nwStream);

        // //---Get the matching player object from the dictionary and inject it into the command----
        // command.TargetPlayer = _players.GetValueOrDefault(command.ClientID);

        //     cm.AddCommand(command);
        //     cm.ProcessPendingTransactions();

        //     Console.WriteLine($"Player {_players[0].playerName} has {_players[0].playerRemainingMoves} moves left.");

        //     client.Close();
        //     listener.Stop();



        // byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes("Welcome Agent 47.");
        // nwStream.Write(bytesToSend, 0, bytesToSend.Length);

        // string dataReceived = null;

        // do
        // {
        //     //---get the incoming data through a network stream---
        //     byte[] buffer = new byte[client.ReceiveBufferSize];

        //     //---read incoming stream---
        //     int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

        //     //---convert the data received into a string---
        //     dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        //     Console.WriteLine("Received : " + dataReceived);


        //     string dataResult = null;


        //     //---write back the text to the client---
        //     Console.WriteLine("Sending back : " + dataResult);
        //     bytesToSend = ASCIIEncoding.ASCII.GetBytes(dataResult);
        //     nwStream.Write(bytesToSend, 0, bytesToSend.Length);
        // } while (!"$exit".Equals(dataReceived));

    }

    // private static void CommandMove(CommandManager cm, Player p, int steps)
    // {
    //     CommandMove mv = new CommandMove(p, steps, "Left");
    //     cm.AddCommand(mv);
    // }

    // private static string CommandExecute(CommandManager cm, Player p)
    // {
    //     cm.ProcessPendingTransactions();

    //     return "Player " + p.playerName + " has " + p.playerRemainingMoves.ToString() + " Moves left.";
    // }
}
