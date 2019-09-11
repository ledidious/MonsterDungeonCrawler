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
using System.Xml.Linq;
using System.Xml;

//TODO: ### CHECK ### LaserBeam mit ROUNDCOUNTER verbinden von der Gameklasse alle % 3 Attribut IsActive auf true setzen -> OnNextRound() bei FieldType
//TODO: ### CHECK ### nach jeder Runde schauen, ob Held ein DefenseItem und/oder AttackItem hat und -- auf Duration, wenn 0 dann Property null setzen und defenseBoost bzw. attackBoost beim Spieler (Hero hat Methode ResetBoost()) zurücksetzen
//TODO: ### CHECK ### Item vom Level löschen, wenn CollectItem() true zurückliefert
//TODO: für die Spieler feste Startpositionen vergeben (Konstruktor sieht XYPositionen bereits vor)
//TODO: Game Klasse muss am Ende jeden AttackCommands prüfen, ob alle noch Leben haben
//TODO: ### CHECK ### immer wenn Spieler an die Reihe kommt, muss Player.PlayerRemainingMoves auf Player.CharacterType._moveRange gesetzt werden
//TODO: ### CHECK ### bevor Spieler dran ist RemainingMoves auf moverange (CharacterType) setzen
//TODO: ### CHECK ### OnNextRound() bei FieldType muss nach jeder Runde bei allen Traps und Items aufgerufen werden - wenn false zurückgeliefert wird, dann muss Item gelöscht werden
//TODO: ### CHECK ### nach jeder Runde muss PlayerList durchlaufen werden und DecrementBoostDuration für DefenseItem und AttackItem vom Player durchgehen, sobald false zurückkommt null setzen

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
        protected Level _level;
        protected int roundsPlayed;
        protected Boolean _gameActive;

        public Game(String sessionID, GameClient firstClient, string levelFileName)
        {
            _clientsOfThisGame.Add(firstClient);
            this._sessionID = sessionID;
            this._currentClient = _clientsOfThisGame[0];

            //TODO: Loader einbauen
            // LoadLevelFile("level1.xml");
            LoadLevelFile(levelFileName + ".xml");

            new Thread(new ThreadStart(() => UpdateClientsInLobby())).Start();

            // _level = new Level(MAX_CLIENTS);
            // for (int x = 0; x <= 19; x++)
            // {
            //     for (int y = 0; y <= 19; y++)
            //     {
            //         _level.AddFieldToLevel(new Field(x, y, new Floor()));
            //     }
            // }

            // CommandManager gcm = new CommandManager();

            // while (true)
            // {
            //     gcm.AddCommand(ReceiveCommandFromClient(_currentClient));
            //     gcm.ProcessPendingTransactions();
            // }
        }

        /// <summary>
        /// Sends update packs at fixed intervals to all clients except the host.
        /// </summary>
        private void UpdateClientsInLobby()
        {
            while (_clientsOfThisGame[0].IsInGame == false)
            {
                foreach (var client in _clientsOfThisGame)
                {
                    if (client.Client_ID != _clientsOfThisGame[0].Client_ID)
                    {
                        if (_level.PlayerList != null && _level.TrapList != null)
                        {
                            UpdatePack update = new UpdatePack(_level.PlayerList, _level.PlayingField, _level.TrapList);
                            if (client.Player.Life > 0)
                            {
                                SendFeedbackToClient(client.TcpClient, new CommandFeedbackUpdatePack(client.Client_ID, true, update));
                            }
                            else
                            {
                                SendFeedbackToClient(client.TcpClient, new CommandFeedbackUpdatePack(client.Client_ID, false, update));
                            }
                        }
                    }
                }

                System.Threading.Thread.Sleep(1000);
            }

            System.Threading.Thread.CurrentThread.Join();
        }

        /// <summary>
        /// Adds a player object to the game
        /// </summary>
        /// <param name="client_ID">ID of the owner of the player</param>
        /// <param name="player">The player to add</param>
        public void AddPlayerToGame(string client_ID, Player player)
        {
            if (_clientsOfThisGame[0].Client_ID == client_ID)
            {
                Hero main = (Hero)player;
                _clientsOfThisGame[0].Player = main;
                _clientsOfThisGame[0].Player.XPosition = 1; _clientsOfThisGame[0].Player.YPosition = 1;
                _level.AddPlayerToLevel(_clientsOfThisGame[0].Player);
                // _players.Add(client_ID, main);
            }
            else
            {
                Monster main = (Monster)player;

                foreach (var client in _clientsOfThisGame)
                {
                    if (client.Client_ID == client_ID)
                    {
                        client.Player = main;
                        switch (_clientsOfThisGame.IndexOf(client))
                        {
                            case 1:
                                client.Player.XPosition = 18;
                                client.Player.YPosition = 1;
                                break;
                            case 2:
                                client.Player.XPosition = 1;
                                client.Player.YPosition = 18;
                                break;
                            case 3:
                                client.Player.XPosition = 18;
                                client.Player.YPosition = 18;
                                break;
                            default:
                                break;
                        }

                        _level.AddPlayerToLevel(client.Player);
                    }
                }
                // _players.Add(client_ID, main);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client_ID"></param>
        public UpdatePack GetUpdatePackForLobby()
        {
            if (_clientsOfThisGame != null)
            {
                return (new UpdatePack(_level.PlayerList, _level.PlayingField, _level.TrapList));
            }
            else
            {
                throw new NotEnoughPlayerInGameException();
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
            //TODO: Wenn Game aktiv, dann nicht zulassen
            if (_clientsOfThisGame[0].Client_ID == client_ID)
            {
                Hero main;
                switch (characterClass)
                {
                    case CharacterClass.Knight:
                        main = new Hero(playerName, new Knight(), 1, 1);
                        break;
                    case CharacterClass.Archer:
                        main = new Hero(playerName, new Archer(), 1, 1);
                        break;
                    default:
                        // main = new Hero("***Error***", new MeleeFighter(), 0, 0);
                        throw new NotImplementedException();
                }

                _clientsOfThisGame[0].Player = main;
                _level.AddPlayerToLevel(_clientsOfThisGame[0].Player);
            }
            else
            {
                Monster villain;
                foreach (var client in _clientsOfThisGame)
                {
                    if (client.Client_ID == client_ID)
                    {
                        switch (characterClass)
                        {
                            case CharacterClass.Knight:
                                villain = new Monster(playerName, new Knight(), 1, 1);
                                break;
                            case CharacterClass.Archer:
                                villain = new Monster(playerName, new Archer(), 1, 1);
                                break;
                            default:
                                // main = new Monster("***Error***", new MeleeFighter(), 0, 0);
                                throw new NotImplementedException();
                        }

                        switch (_clientsOfThisGame.IndexOf(client))
                        {
                            case 1:
                                villain.XPosition = 18;
                                villain.YPosition = 1;
                                break;
                            case 2:
                                villain.XPosition = 1;
                                villain.YPosition = 18;
                                break;
                            case 3:
                                villain.XPosition = 18;
                                villain.YPosition = 18;
                                break;
                            default:
                                break;
                        }

                        client.Player = villain;
                        _level.AddPlayerToLevel(client.Player);
                        Console.WriteLine("Villain Position: " + client.Player.XPosition + ", " + client.Player.YPosition);
                    }
                }
            }

            /*  if (_clientsOfThisGame.Count == MAX_CLIENTS)
             {
                 if (_level.PlayerList.Count == MAX_CLIENTS)
                 {
                     UpdateClients();
                     SendFeedbackToClient(_clientsOfThisGame[0].TcpClient, new CommandFeedbackYourTurn(_clientsOfThisGame[0].Client_ID));
                 }
             }
             else
             {
                 UpdateClients();
             }*/
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
                throw new GameLobbyIsFullException();
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
                roundsPlayed = 0;

                //TODO: Verhindern, dass ein Spiel mehrfach gestartet werden kann.
                while (_clientsOfThisGame[0].TcpClient.Connected)
                {
                    Console.WriteLine("Du bist dran " + _currentClient.Player.PlayerName);
                    Console.WriteLine(_currentClient.Player.PlayerName + " has " + _currentClient.Player.PlayerRemainingMoves + " moves");

                    if (_currentClient.Player.Life > 0)
                    {
                        do
                        {
                            Console.WriteLine("Moves left: " + _currentClient.Player.PlayerRemainingMoves);
                            Console.WriteLine("Position: " + _currentClient.Player.XPosition + ", " + _currentClient.Player.YPosition);
                            CommandGame command = ReceiveCommandFromClient(_currentClient.TcpClient);
                            // command.SourcePlayer = _players.GetValueOrDefault(command.SourceClientID);
                            command.SourcePlayer = _currentClient.Player;
                            command.Level = _level;

                            try
                            {
                                command.Execute();

                                if (_currentClient.Player.PlayerRemainingMoves > 0)
                                {
                                    SendFeedbackToClient(_currentClient.TcpClient, new CommandFeedbackOK(_currentClient.Client_ID));
                                }
                                else
                                {
                                    SendFeedbackToClient(_currentClient.TcpClient, new CommandFeedbackEndOfTurn(_currentClient.Client_ID));
                                }

                            }
                            catch (System.Exception e)
                            {
                                SendFeedbackToClient(_currentClient.TcpClient, new CommandFeedbackGameException(_currentClient.Client_ID, e));
                            }

                        } while (_currentClient.Player.PlayerRemainingMoves > 0);
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

            // set the binder to the custom binder:
            formatter.Binder = TypeOnlyBinder.Default;

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

            // set the binder to the custom binder:
            formatter.Binder = TypeOnlyBinder.Default;

            var ms = new MemoryStream();
            formatter.Serialize(ms, command);

            byte[] bytesToSend = ms.ToArray();

            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
            nwStream.Flush();
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateClientsInGame()
        {
            foreach (var client in _clientsOfThisGame)
            {
                UpdatePack update = new UpdatePack(_level.PlayerList, _level.PlayingField, _level.TrapList);
                if (client.Player.Life > 0)
                {
                    SendFeedbackToClient(client.TcpClient, new CommandFeedbackUpdatePack(client.Client_ID, true, update));
                }
                else
                {
                    SendFeedbackToClient(client.TcpClient, new CommandFeedbackUpdatePack(client.Client_ID, false, update));
                }
            }
        }

        /// <summary>
        /// Ends the turn of a client
        /// </summary>
        /// <returns>TODO: Muss hier etwas zurückgegeben werden, oder muss nur auf den nächsten Client gehorcht werden?</returns>
        public void NextPlayer()
        {
            if (_currentClient.Player is Hero)
            {
                foreach (var item in _level.PlayingField)
                {
                    if (item.FieldType is Exit && item.XPosition == _currentClient.Player.XPosition && item.YPosition == _currentClient.Player.YPosition && _currentClient.Player.HasKey)
                    {
                        foreach (var client in _clientsOfThisGame)
                        {
                            //SendFeedbackToClient(client.TcpClient, new CommandFeedbackEndOfGame());
                            //Oder ein UpdatePack welches alle über Ende des Spiels informiert
                        }
                        //TODO: Held gewinnen lassen
                    }
                }

            }

            if (_clientsOfThisGame[1].Player.Life <= 0 && _clientsOfThisGame[2].Player.Life <= 0 && _clientsOfThisGame[3].Player.Life <= 0)
            {
                //TODO: Held gewinnen lassen
            }

            if (_clientsOfThisGame[0].Player.Life <= 0)
            {
                //TODO: Monster gewinnen lassen
            }

            _currentClient.Player.ResetRemainingMoves();

            if (_clientsOfThisGame.IndexOf(_currentClient) < (MAX_CLIENTS - 1))
            {
                _currentClient = _clientsOfThisGame[_clientsOfThisGame.IndexOf(_currentClient) + 1];
            }
            else
            {
                _currentClient = _clientsOfThisGame[0];
                ItemManagement();
                roundsPlayed++;
            }

            if ((roundsPlayed % 2) == 0)
            {
                foreach (var field in _level.TrapList)
                {
                    field.FieldType.OnNextRound();
                }
            }

            UpdateClientsInGame();
            SendFeedbackToClient(_currentClient.TcpClient, new CommandFeedbackYourTurn(_currentClient.Client_ID));
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
                if (_level.PlayerList[i].AttackItem != null)
                {
                    if (_level.PlayerList[i].AttackItem.DecrementBoosterDuration() == false)
                    {
                        _level.PlayerList[i].ResetAttackBooster();
                        _level.PlayerList[i].ResetAttackItem();
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

                if (_level.PlayerList[i].DefenseItem != null)
                {
                    if (_level.PlayerList[i].DefenseItem.DecrementBoosterDuration() == false)
                    {
                        _level.PlayerList[i].ResetDefenseBooster();
                        _level.PlayerList[i].ResetDefenseItem();
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

        private void LoadLevelFile(string file)
        {
            //TODO: Evtl. Pfad für verschiedene OS anpassen
            var fullPathToFile = Path.Combine(Path.GetFullPath(Directory.GetCurrentDirectory()), ("Level" + Path.DirectorySeparatorChar + file));

            XElement levelFromFile = XElement.Load(fullPathToFile);
            // XElement test = new XElement(levelFromFile.Name,levelFromFile.)

            // Create the level object
            _level = new Level(MAX_CLIENTS, Int32.Parse(levelFromFile.Attribute("width").Value));

            // foreach (var item in levelFromFile.Attributes())
            // {
            //     if (item.Name == "width")
            //     {
            //         _level = new Level(MAX_CLIENTS, Int32.Parse(item.Value));
            //     }
            // }

            // Read field information.
            foreach (var item in levelFromFile.Elements())
            {
                if (item.Name == "Field")
                {
                    if (item.Element("Wall") != null)
                    {
                        _level.AddFieldToLevel(new Field(Int32.Parse(item.Attribute("x").Value), Int32.Parse(item.Attribute("y").Value), new Wall()));
                    }
                    else if (item.Element("SpikeField") != null)
                    {
                        _level.AddFieldToLevel(new Field(Int32.Parse(item.Attribute("x").Value), Int32.Parse(item.Attribute("y").Value), new SpikeField()));
                    }
                    else if (item.Element("Trapdoor") != null)
                    {
                        _level.AddFieldToLevel(new Field(Int32.Parse(item.Attribute("x").Value), Int32.Parse(item.Attribute("y").Value), new Trapdoor()));
                    }
                    else if (item.Element("Laserbeam") != null)
                    {
                        _level.AddFieldToLevel(new Field(Int32.Parse(item.Attribute("x").Value), Int32.Parse(item.Attribute("y").Value), new LaserBeam()));
                    }
                    else if (item.Element("Exit") != null)
                    {
                        _level.AddFieldToLevel(new Field(Int32.Parse(item.Attribute("x").Value), Int32.Parse(item.Attribute("y").Value), new Exit()));
                    }
                    else if (item.Element("Sword") != null)
                    {
                        Field boostField = new Field(Int32.Parse(item.Attribute("x").Value), Int32.Parse(item.Attribute("y").Value), new Floor());
                        boostField.Item = new AttackBoost(Int32.Parse(item.Element("Sword").Attribute("level").Value));
                        _level.AddFieldToLevel(boostField);
                    }
                    else if (item.Element("Shield") != null)
                    {
                        Field boostField = new Field(Int32.Parse(item.Attribute("x").Value), Int32.Parse(item.Attribute("y").Value), new Floor());
                        boostField.Item = new DefenseBoost(Int32.Parse(item.Element("Shield").Attribute("level").Value));
                        _level.AddFieldToLevel(boostField);
                    }
                    else if (item.Element("Key") != null)
                    {
                        Field keyField = new Field(Int32.Parse(item.Attribute("x").Value), Int32.Parse(item.Attribute("y").Value), new Floor());
                        keyField.Item = Key.getInstance();
                        _level.AddFieldToLevel(keyField);
                    }
                    else if (item.Element("Heart") != null)
                    {
                        Field heartField = new Field(Int32.Parse(item.Attribute("x").Value), Int32.Parse(item.Attribute("y").Value), new Floor());
                        heartField.Item = new ExtraLife();
                        _level.AddFieldToLevel(heartField);
                    }
                    else if (item.Element("Hero") != null)
                    {
                        _level.AddFieldToLevel(new Field(Int32.Parse(item.Attribute("x").Value), Int32.Parse(item.Attribute("y").Value), new Floor()));
                    }
                    else
                    {
                        _level.AddFieldToLevel(new Field(Int32.Parse(item.Attribute("x").Value), Int32.Parse(item.Attribute("y").Value), new Floor()));
                    }
                }
            }

            // foreach (var item in levelFromFile.Elements())
            // {
            //     foreach (var franz in item.Elements())
            //     {
            //         Console.WriteLine(franz);
            //     }
            // }
        }
    }
}
