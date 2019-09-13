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
using GameLogic.MDC.Gamedata;

namespace GameLogic.MDC.Server
{
    public class MasterServer
    {
        static Int32 PORT_NO;
        static string SERVER_IP;
        private static TcpListener listener;
        public static Boolean Shutdown { get; set; }
        private List<Thread> myThreads;

        static private Dictionary<string, Game> _games = new Dictionary<string, Game>(); //sessionID, Game
        static private Dictionary<string, GameClient> _gClients = new Dictionary<string, GameClient>(); //clientID, TcpClient
        static private CommandManager scm = new CommandManager();

        /// <summary>
        /// Prepare the server for the connection of new clients.
        /// </summary>
        public static void StartServer()
        {
            ReadConfig();
            // CommandManager scm = new CommandManager();

            //---listen at the specified IP and port no.---
            IPAddress localAdd = IPAddress.Any; //IPAddress.Parse(SERVER_IP);
            listener = new TcpListener(localAdd, PORT_NO);

            // Console.WriteLine("IP: " + SERVER_IP);
            Console.WriteLine("##################\n MDC MasterServer \n##################");
            Console.WriteLine("Port: " + PORT_NO);
            Console.WriteLine("Listening...");

            Shutdown = false;

            Thread serverThread = new Thread(new ThreadStart(() => ServerOperation(listener)));
            serverThread.Start();

            // // Keep the Server alive
            // while (Shutdown == false)
            // {

            // }

            // while (Shutdown == false)
            // {
            //     TcpClient tcpClient = listener.AcceptTcpClient();

            //     Thread clientThread = new Thread(new ThreadStart(() => ClientInteraction(tcpClient, new CommandManager())));
            //     clientThread.Start();
            // }



            // listener.Stop();
        }

        private static void ServerOperation(TcpListener listener)
        {
            listener.Start();
            while (Shutdown == false)
            {
                TcpClient tcpClient = listener.AcceptTcpClient();
                tcpClient.SendBufferSize = 524288;
                tcpClient.ReceiveBufferSize = 524288;

                Thread clientThread = new Thread(new ThreadStart(() => ClientInteraction(tcpClient, new CommandManager())));
                clientThread.Start();
            }
            Console.WriteLine("Penis");

            // System.Threading.Thread.CurrentThread.Abort();
            // System.Threading.Thread.CurrentThread.Join();            
        }

        public static void StopServer()
        {
            Console.WriteLine("Shuting down Server...");
            Shutdown = true;
            // listener.Stop();

            foreach (var item in _gClients)
            {
                item.Value.TcpClient.Close();
            }

            // ProcessThreadCollection currentThreads = Process.GetCurrentProcess().Threads;

            // foreach (ProcessThread thread in currentThreads)
            // {
            //     thread.Id
            // }
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
        public static void CreateNewGame(string client_ID, string levelFileName)
        {
            string session_ID = GenerateID();
            _games.Add(session_ID, new Game(session_ID, _gClients[client_ID], levelFileName));

            SendFeedbackToClient(_gClients[client_ID].TcpClient, new CommandFeedbackOK(client_ID));
            SendStringToClient(_gClients[client_ID].TcpClient, session_ID);
            _gClients[client_ID].IsHost = true;
        }

        /// <summary>
        /// [Only called by received commands] Connects a client to an existing game session.
        /// </summary>
        /// <param name="client_ID">ID of the executing client</param>
        /// <param name="session_ID">ID of the game the client wants to connect to</param>
        public static void ConnectClientToGame(string client_ID, string session_ID)
        {
            Console.WriteLine("Connecting to " + session_ID + " with client " + client_ID);

            if (_games.ContainsKey(session_ID))
            {
                try
                {
                    _games[session_ID].AddClientToGame(_gClients[client_ID]);
                    SendFeedbackToClient(_gClients[client_ID].TcpClient, new CommandFeedbackOK(client_ID));
                }
                catch (System.Exception e)
                {
                    SendFeedbackToClient(_gClients[client_ID].TcpClient, new CommandFeedbackGameException(client_ID, e));
                }
            }
            else
            {
                SendFeedbackToClient(_gClients[client_ID].TcpClient, new CommandFeedbackGameException(client_ID, new SessionIdIsInvalidException()));
            }
        }

        /// <summary>
        /// Forwards a player object created by the command to the correct game session.
        /// </summary>
        /// <param name="client_ID">ID of the executing client</param>
        /// <param name="session_ID">ID of the game the client wants to connect to</param>
        /// <param name="player">The player object to be forwarded</param>
        public static void CreateNewPlayerForSession(string client_ID, string session_ID, string playerName, CharacterClass characterClass)
        {
            try
            {
                _games[session_ID].AddPlayerToGame(client_ID, playerName, characterClass);

                if (_gClients[client_ID].IsHost == false)
                {
                    SendFeedbackToClient(_gClients[client_ID].TcpClient, new CommandFeedbackEndOfTurn(client_ID));
                    _gClients[client_ID].IsInGame = true;
                }
                else
                {
                    SendFeedbackToClient(_gClients[client_ID].TcpClient, new CommandFeedbackOK(client_ID));
                }
            }
            catch (System.Exception e)
            {
                SendFeedbackToClient(_gClients[client_ID].TcpClient, new CommandFeedbackGameException(client_ID, e));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client_ID"></param>
        /// <param name="session_ID"></param>
        public static void SendUpdatePackForLobby(string client_ID, string session_ID)
        {
            try
            {
                if (_gClients[client_ID].IsHost)
                {
                    Console.WriteLine("Benis");
                    UpdatePack pp = _games[session_ID].GetUpdatePackForLobby();
                    SendFeedbackToClient(_gClients[client_ID].TcpClient, new CommandFeedbackUpdatePack(client_ID, true, pp));
                }
            }
            catch (System.Exception e)
            {
                SendFeedbackToClient(_gClients[client_ID].TcpClient, new CommandFeedbackGameException(client_ID, e));
            }
        }

        /// <summary>
        /// [Only called by received commands] Starts the game round. Executable only when all players are connected.
        /// </summary>
        /// <param name="session_ID">ID of the game session to be started</param>
        public static void StartGame(string client_ID, string session_ID)
        {
            try
            {
                // foreach (var item in _games.GetValueOrDefault(session_ID).ClientsOfThisGame)
                // {
                //     item.IsInGame = true;
                // }
                Thread gameThread = new Thread(new ThreadStart(() => _games[session_ID].StartGame()));
                gameThread.Start();
                // _games.GetValueOrDefault(session_ID).StartGame();
                SendFeedbackToClient(_gClients[client_ID].TcpClient, new CommandFeedbackOK(client_ID));
                _gClients[client_ID].IsInGame = true;
            }
            catch (NotEnoughPlayerInGameException e)
            {
                //TODO: Evtl. noch andere Excepptions abfangen, da alles über diese Methode läuft?!
                throw e;
            }
        }

        /// <summary>
        /// Thread implementation for clients. Waits for commands from client. 
        /// </summary>
        /// <param name="client">The TcpClient which belongs to this thread</param>
        /// <param name="cm">The server-wide CommandManager</param>
        private static void ClientInteraction(TcpClient client, CommandManager cm)
        {
            // string client_ID = GenerateID();
            GameClient gClient = new GameClient(client, GenerateID());
            _gClients.Add(gClient.Client_ID, gClient);

            //---write back the client ID to the client---
            SendStringToClient(gClient.TcpClient, gClient.Client_ID);

            //TODO: GameClient in dieser Klasse implementieren. Sobald StartGame aufgerufen wird, verlassen die zum Spiel gehörenden Clients diesen loop
            while (gClient.TcpClient.Connected)
            {
                while (gClient.IsInGame == false)
                {
                    Console.WriteLine("\n ------------------------ \n Waiting for Command... \n ------------------------");
                    Command command = ReceiveCommandFromClient(gClient.TcpClient);
                    if (command != null)
                    {
                        // leCommand.Execute();
                        cm.AddCommand(command);
                        cm.ProcessPendingTransactions();
                    }
                }
            }
            // gClient.TcpClient.Close();
            Console.WriteLine("Client Penis");
            System.Threading.Thread.CurrentThread.Abort();
            System.Threading.Thread.CurrentThread.Join();
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
            Byte[] bytesToRead = new byte[client.ReceiveBufferSize];
            Int32 bytesRead = nwStream.Read(bytesToRead, 0, bytesToRead.Length);

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
            Byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(data);
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
            //MemoryStream dataStream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();

           /* byte[] bytesToRead = new byte[client.ReceiveBufferSize];
            int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);

            dataStream.Write(bytesToRead, 0, bytesToRead.Length);
            dataStream.Seek(0, SeekOrigin.Begin); */

            try
            {
                var obj = formatter.Deserialize(nwStream);
                Console.WriteLine("MASTERSERVER: " + obj.GetType());
                nwStream.Flush();
                if (obj.GetType().IsSubclassOf(typeof(Command)))
                {
                    return (Command)obj;
                }
            }
            catch (System.Runtime.Serialization.SerializationException e)
            {
                Console.WriteLine(e.Message);
            }

            nwStream.Flush();
            return null;
        }

        /// <summary>
        /// Send Command to client
        /// </summary>
        /// <param name="server">TcpClient to which data is to be sent.</param>
        /// <param name="command">Command you want to send</param>
        private static void SendFeedbackToClient(TcpClient client, CommandFeedback command)
        {
            NetworkStream nwStream = client.GetStream();
            //MemoryStream dataStream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();

            // set the binder to the custom binder:
            //formatter.Binder = TypeOnlyBinder.Default;

            /* var ms = new MemoryStream();
             formatter.Serialize(ms, command);
             ms.Flush(); //TODO: Evtl. entfernen
             ms.Position = 0; //TODO: Evtl. entfernen

             byte[] bytesToSend = ms.ToArray();
             ms.Close(); //TODO: Evtl. entfernen

             nwStream.Write(bytesToSend, 0, bytesToSend.Length);*/
            formatter.Serialize(nwStream, command);
            nwStream.Flush(); 
        }
    }
}
