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
    public class Server
    {
        const int PORT_NO = 5000;
        const string SERVER_IP = "127.0.0.1";
        const int MAX_CLIENTS = 4;

        // Player/Client mapping: the string represents the clientID
        static private Dictionary<string, Player> _players = new Dictionary<string, Player>();

        /// <summary>
        /// Start a new server, which clients can connect to via a TCP connection
        /// </summary>
        public static void StartServer(String server_ip, int port_no)
        {

            CommandManager cm = new CommandManager();

            //---listen at the specified IP and port no.---
            IPAddress localAdd = IPAddress.Parse(server_ip);
            TcpListener listener = new TcpListener(localAdd, port_no);
            
            Console.WriteLine("IP: " + SERVER_IP);
            Console.WriteLine("Listening...");
            listener.Start();

            int counter = 0;
            while (counter < MAX_CLIENTS)
            {
                counter++;
                TcpClient tcpClient = listener.AcceptTcpClient();

                Thread clientThread = new Thread(new ThreadStart(() => ClientInteraction(tcpClient, cm)));
                clientThread.Start();
            }

            listener.Stop();
        }

        /// <summary>
        /// Called when a new Client connects to the server
        /// </summary>
        /// <param name="client">The Tcp Client</param>
        /// <param name="cm">The Command Manager</param>
        private static void ClientInteraction(TcpClient client, CommandManager cm)
        {
            string clientID = GenerateID();

            //---Get the playerName from the server and create a new Player---
            string playerName = ReceiveStringFromClient(client);
            // _players.Add(clientID, new Player(playerName, 20));
            //TODO: Make it universal --> Each PlayerType should be addable
            _players.Add(clientID, new Hero(playerName, 20));

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
        private static GameCommand ReceiveCommandFromClient(TcpClient client)
        {
            NetworkStream nwStream = client.GetStream();
            IFormatter formatter = new BinaryFormatter();

            return (GameCommand)formatter.Deserialize(nwStream); ;
        }

        // ############
        // # OLD CODE #
        // ############


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
