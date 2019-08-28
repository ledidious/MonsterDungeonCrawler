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
using MDC.Exceptions;
using MDC.Gamedata;
using MDC.Gamedata.PlayerType;
using MDC.Gamedata.LevelContent;

//TODO: LaserBeam mit ROUNDCOUNTER verbinden von der Gameklasse alle % 3 Attribut IsActive auf true setzen -> OnNextRound() bei FieldType
//TODO: nach jeder Runde schauen, ob Held ein DefenseItem und/oder AttackItem hat und -- auf Duration, wenn 0 dann Property null setzen und defenseBoost bzw. attackBoost beim Spieler (Hero hat Methode ResetBoost()) zurücksetzen
//TODO: Item vom Level löschen, wenn CollectItem() true zurückliefert
//TODO: für die Spieler feste Startpositionen vergeben (Konstruktor sieht XYPositionen bereits vor)
//TODO: Game Klasse muss am Ende jeden AttackCommands prüfen, ob alle noch Leben haben
//TODO: immer wenn Spieler an die Reihe kommt, muss Player.PlayerRemainingMoves auf Player.CharacterType._moveRange gesetzt werden
//TODO: bevor Spieler dran ist RemainingMoves auf moverange (CharacterType) setzen
//TODO: OnNextRound() bei FieldType muss nach jeder Runde bei allen Traps und Items aufgerufen werden - wenn false zurückgeliefert wird, dann muss Item gelöscht werden
//TODO: nach jeder Runde muss PlayerList durchlaufen werden und DecrementBoostDuration für DefenseItem und AttackItem vom Player durchgehen, sobald false zurückkommt null setzen

namespace MDC.Server
{
    public class Game
    {
        public int RoundCounter;
        const int MAX_CLIENTS = 4;
        private int PORT_NO_SESSION;

        // Player/Client mapping: the string represents the clientID
        // private Dictionary<string, Player> _players = new Dictionary<string, Player>(); TODO: !!!
        // private Dictionary<string, TcpClient> _clientsOfThisGame = new Dictionary<string, TcpClient>();
        private List<GameClient> _clientsOfThisGame = new List<GameClient>();
        public List<GameClient> ClientsOfThisGame { get { return _clientsOfThisGame; } }
        private GameClient _currentClient;
        // private List<TcpListener> _test = new List<TcpClient>();
        // private Player _currentPlayer;
        private int _lapsRemaining;
        private String _sessionID;
        CommandManager gcm = new CommandManager();
        protected Level _board;

        public Game(String sessionID, GameClient firstClient)
        {
            _clientsOfThisGame.Add(firstClient);
            this._sessionID = sessionID;
            this._currentClient = _clientsOfThisGame[0];


            _board = new Level(MAX_CLIENTS);

            //TODO: Loader einbauen
            for (int x = 0; x < 19; x++)
            {
                for (int y = 0; y < 19; y++)
                {
                    _board.AddFieldToLevel(new Field(x, y, new Floor()));
                }
            }
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
            //TODO: Richtige Positionen vergeben!!!

            if (_clientsOfThisGame[0].Player == null)
            {
                Hero main = (Hero)player;
                _clientsOfThisGame[0].Player = main;
                _clientsOfThisGame[0].Player.XPosition = 1; _clientsOfThisGame[0].Player.YPosition = 1;
                _board.AddPlayerToLevel(_clientsOfThisGame[0].Player);
                // _players.Add(client_ID, main);
            }
            else
            {
                Monster main = (Monster)player;

                int index = 0;
                foreach (var client in _clientsOfThisGame)
                {
                    if (client.Client_ID == client_ID)
                    {
                        client.Player = main;
                        switch (index)
                        {
                            case 1:
                                client.Player.XPosition = 18;
                                client.Player.YPosition = 1;
                                break;
                            case 2:
                                client.Player.XPosition = 18;
                                client.Player.YPosition = 1;
                                break;
                            case 3:
                                client.Player.XPosition = 18;
                                client.Player.YPosition = 18;
                                break;
                            default:
                                break;
                        }

                        _board.AddPlayerToLevel(client.Player);
                    }
                    index++;
                }
                // _players.Add(client_ID, main);
            }
        }

        /// <summary>
        /// Creates a new player and adds it to the game
        /// </summary>
        /// <param name="client_ID">ID of the owner of the player</param>
        /// <param name="playerName">Name of the new player</param>
        /// <param name="characterClass">The class of character of the character</param>
        public void AddPlayerToGame(string client_ID, string playerName, CharacterClass characterClass)
        {
            if (_clientsOfThisGame[0].Player == null)
            {
                Hero main;
                switch (characterClass)
                {
                    case CharacterClass.MeleeFighter:
                        main = new Hero(playerName, new MeleeFighter(), 0, 0);
                        break;
                    case CharacterClass.RangeFighter:
                        main = new Hero(playerName, new RangeFighter(), 0, 0);
                        break;
                    default:
                        main = new Hero("***Error***", new MeleeFighter(), 0, 0);
                        break;
                }

                _clientsOfThisGame[0].Player = main;
            }
            else
            {
                Monster main;
                foreach (var client in _clientsOfThisGame)
                {
                    if (client.Client_ID == client_ID)
                    {
                        switch (characterClass)
                        {
                            case CharacterClass.MeleeFighter:
                                main = new Monster(playerName, new MeleeFighter(), 0, 0);
                                break;
                            case CharacterClass.RangeFighter:
                                main = new Monster(playerName, new RangeFighter(), 0, 0);
                                break;
                            default:
                                main = new Monster("***Error***", new MeleeFighter(), 0, 0);
                                break;
                        }
                        client.Player = main;
                    }
                }
                // _players.Add(client_ID, main);
            }
            // switch (characterClass)
            // {

            //     default:
            // }
            // _players.Add()
        }

        /// <summary>
        /// Adds a TcpClient to the game
        /// </summary>
        /// <param name="client">The TcpClient</param>
        public void AddClientToGame(GameClient gClient)
        {
            if (_clientsOfThisGame.Count < MAX_CLIENTS)
            {
                _clientsOfThisGame.Add(gClient);
            }
            else
            {
                throw new NotImplementedException(); //TODO: Melden, dass maximale Anzahl an Clients bereits erreicht wurde
            }
        }

        /// <summary>
        /// [Call only when all players are ready] Starts the game round
        /// </summary>
        public void StartGame()
        {
            if (_clientsOfThisGame.Count < 4)
            {
                throw new NotEnoughPlayerInGameException();
            }
            else
            {
                // foreach (var gClient in _clientsOfThisGame)
                // {
                //     _board.AddPlayerToLevel(gClient.Player);
                // }

                //TODO: Vor Eintritt in die Schleife, den Host erst eine Runde spielen lassen
                while (_clientsOfThisGame[0].TcpClient.Connected)
                {
                    Console.WriteLine("Du bist dran " + _currentClient.Player.PlayerName);
                    while (_currentClient.Player.PlayerRemainingMoves > 0)
                    {
                        Console.WriteLine("Moves left: " + _currentClient.Player.PlayerRemainingMoves);
                        Console.WriteLine("Position: " + _currentClient.Player.XPosition + ", " + _currentClient.Player.YPosition);
                        CommandGame command = ReceiveCommandFromClient(_currentClient.TcpClient);
                        // command.SourcePlayer = _players.GetValueOrDefault(command.SourceClientID);
                        command.SourcePlayer = _currentClient.Player;
                        command.level = _board;

                        command.Execute();
                        SendFeedbackToClient(_currentClient.TcpClient, new CommandFeedbackOK(_currentClient.Client_ID));
                    }
                    NextPlayer();

                }

                throw new NotImplementedException();
            }
            // gcm.AddCommand(ReceiveCommandFromClient(_currentClient));
            // gcm.ProcessPendingTransactions();
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
            // NetworkStream nwStream = client.GetStream();
            // IFormatter formatter = new BinaryFormatter();

            // return (CommandGame)formatter.Deserialize(nwStream);

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
                Console.WriteLine("GAME: " + obj.GetType());
                if (obj.GetType().IsSubclassOf(typeof(CommandGame)))
                {
                    return (CommandGame)obj;
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
        private static void SendFeedbackToClient(TcpClient client, CommandFeedback command)
        {
            NetworkStream nwStream = client.GetStream();
            MemoryStream dataStream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();

            var ms = new MemoryStream();
            formatter.Serialize(ms, command);

            byte[] bytesToSend = ms.ToArray();

            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
            nwStream.Flush();
        }

        /// <summary>
        /// Ends the turn of a client
        /// </summary>
        /// <returns>TODO: Muss hier etwas zurückgegeben werden, oder muss nur auf den nächsten Client gehorcht werden?</returns>
        public void NextPlayer()
        {
            if (_clientsOfThisGame.IndexOf(_currentClient) < (MAX_CLIENTS - 1))
            {
                _currentClient = _clientsOfThisGame[_clientsOfThisGame.IndexOf(_currentClient) + 1];
            }
            else
            {
                _currentClient = _clientsOfThisGame[0];
            }
            // _currentClient = _clientsOfThisGame.FindIndex();
            // return null;
        }

        /// <summary>
        /// Read the configuration from the config-file and store the value in the corresponding variables.
        /// </summary>
        private void ReadConfig()
        {
            var data = new Dictionary<string, string>();
            foreach (var row in File.ReadAllLines("game.config"))
                data.Add(row.Split('=')[0], string.Join("=", row.Split('=').Skip(1).ToArray()));

            PORT_NO_SESSION = Convert.ToInt32(data["PortNo"]) + 1; //Games should not listen on the same port as the MasterServer
        }

        //TODO: call after every round
        private void ItemManagement()
        {
            for (int i = 0; i < MAX_CLIENTS; i++)
            {
                if (_board.playerList[i].AttackItem != null)
                {
                    if (_board.playerList[i].AttackItem.DecrementBoosterDuration() == false)
                    {
                        _board.playerList[i].ResetAttackBooster();
                        _board.playerList[i].ResetAttackItem();
                    }
                    else
                    {
                        //itemduration is not 0
                    }
                }
                else
                {
                    //no attackitem available
                }

                if (_board.playerList[i].DefenseItem != null)
                {
                    if (_board.playerList[i].DefenseItem.DecrementBoosterDuration() == false)
                    {
                        _board.playerList[i].ResetDefenseBooster();
                        _board.playerList[i].ResetDefenseItem();
                    }
                    else
                    {
                        //itemduration is not 0
                    }
                }
                else
                {
                    //no defenseitem available
                }
            }
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
