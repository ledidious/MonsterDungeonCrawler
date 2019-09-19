using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Linq;
using GameLogic.MDC.Gamedata;
using GameLogic.MDC.Gamedata.PlayerType;
using GameLogic.MDC.Gamedata.LevelContent;
using System.Threading;

namespace GameLogic.MDC.Server
{
    public class Game
    {
        public int RoundCounter;
        const int MAX_CLIENTS = 4;
        private int PORT_NO_SESSION;

        private List<GameClient> _clientsOfThisGame = new List<GameClient>();
        public List<GameClient> ClientsOfThisGame { get { return _clientsOfThisGame; } }
        private GameClient _currentClient;
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

            LoadLevelFile(levelFileName + ".xml");

            new Thread(new ThreadStart(() => UpdateClientsInLobby())).Start();
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
            }
        }

        /// <summary>
        /// Gets the update pack for lobby.
        /// </summary>
        /// <returns>The update pack for lobby.</returns>
        public UpdatePack GetUpdatePackForLobby()
        {
            if (_clientsOfThisGame != null)
            {
                return (new UpdatePack(RoundCounter ,_currentClient.Client_ID, CreatePlayerClientMapping(), _level.PlayingField, _level.TrapList, null));
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
                    }
                }
            }
        }

        /// <summary>
        /// Sends update packs at fixed intervals to all clients except the host.
        /// </summary>
        private void UpdateClientsInLobby()
        {
            while (_clientsOfThisGame[0].IsInGame == false)
            {
                try
                {
                    foreach (var client in _clientsOfThisGame)
                    {
                        if (client.Client_ID != _clientsOfThisGame[0].Client_ID && client.Player != null)
                        {
                            if (_level.PlayerList != null && _level.TrapList != null)
                            {
                                UpdatePack update = new UpdatePack(RoundCounter, _currentClient.Client_ID, CreatePlayerClientMapping(), _level.PlayingField, _level.TrapList, null);
                                if (client.Player.Life > 0)
                                {
                                    SendFeedbackToClient(client.TcpClient, new CommandFeedbackUpdatePack(client.Client_ID, true, update));
                                    Console.WriteLine("Sending Update to: " + client.Player.PlayerName);
                                }
                                else
                                {
                                    SendFeedbackToClient(client.TcpClient, new CommandFeedbackUpdatePack(client.Client_ID, false, update));
                                }
                            }
                        }
                    }
                }
                catch (System.Exception)
                { }

                System.Threading.Thread.Sleep(1000);
            }

            //Send last Update which informs Clients to load the level
            foreach (var client in _clientsOfThisGame)
            {
                if (client.Client_ID != _clientsOfThisGame[0].Client_ID && client.Player != null)
                {
                    if (_level.PlayerList != null && _level.TrapList != null)
                    {
                        UpdatePack update = new UpdatePack(RoundCounter, _currentClient.Client_ID, CreatePlayerClientMapping(), _level.PlayingField, _level.TrapList, _level.LevelName);
                        if (client.Player.Life > 0)
                        {
                            SendFeedbackToClient(client.TcpClient, new CommandFeedbackUpdatePack(client.Client_ID, true, update));
                            Console.WriteLine("Sending Update to: " + client.Player.PlayerName);
                        }
                        else
                        {
                            SendFeedbackToClient(client.TcpClient, new CommandFeedbackUpdatePack(client.Client_ID, false, update));
                        }
                    }
                }
            }



            System.Threading.Thread.CurrentThread.Join();
        }

        /// <summary>
        /// Adds a TcpClient to the game
        /// </summary>
        /// <param name="gClient">The GameClient to add</param>
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
            if (_level.PlayerList.Count < 4)
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
                            command.SourcePlayer = _currentClient.Player;
                            command.Level = _level;

                            try
                            {
                                command.Execute();

                                if (_currentClient.Player.PlayerRemainingMoves > 0)
                                {
                                    SendFeedbackToClient(_currentClient.TcpClient, new CommandFeedbackOK(_currentClient.Client_ID));
                                    //System.Threading.Thread.Sleep(100);
                                    UpdateClients();
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
        private static CommandGame ReceiveCommandFromClient(TcpClient client)
        {
            NetworkStream nwStream = client.GetStream();
            IFormatter formatter = new BinaryFormatter();

            try
            {
                var obj = formatter.Deserialize(nwStream);
                Console.WriteLine("GAME: " + obj.GetType());
                nwStream.Flush();
                if (obj.GetType().IsSubclassOf(typeof(CommandGame)))
                {
                    return (CommandGame)obj;
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
        /// <param name="command">Command you want to send</param>
        private static void SendFeedbackToClient(TcpClient client, CommandFeedback command)
        {
            NetworkStream nwStream = client.GetStream();
            IFormatter formatter = new BinaryFormatter();

            formatter.Serialize(nwStream, command);
            nwStream.Flush();
        }

        /// <summary>
        /// Updates the clients.
        /// </summary>
        private void UpdateClients()
        {
            foreach (var client in _clientsOfThisGame)
            {
                UpdatePack update = new UpdatePack(RoundCounter, _currentClient.Client_ID, CreatePlayerClientMapping(), _level.PlayingField, _level.TrapList, null);
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
        /// Ends the active player's turn and checks:
        /// - Whether a player has won
        /// - The durability of the items
        /// Notifies the next player
        /// </summary>
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

            UpdateClients();
            SendFeedbackToClient(_currentClient.TcpClient, new CommandFeedbackYourTurn(_currentClient.Client_ID));
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

        /// <summary>
        /// Loads the level file.
        /// </summary>
        /// <param name="fileName">Filename of the level (without extension).</param>
        private void LoadLevelFile(string fileName)
        {
            var fullPathToFile = Path.Combine(Path.GetFullPath(Directory.GetCurrentDirectory()), ("Level" + Path.DirectorySeparatorChar + fileName));

            XElement levelFromFile = XElement.Load(fullPathToFile);

            // Create the level object
            _level = new Level(MAX_CLIENTS, Int32.Parse(levelFromFile.Attribute("width").Value), (fileName.Split('.')[0]));

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
        }

        /// <summary>
        /// Creates the player client mapping.
        /// </summary>
        /// <returns>The player client mapping.</returns>
        private List<PlayerClientMapping> CreatePlayerClientMapping()
        {
            List<PlayerClientMapping> tmpList = new List<PlayerClientMapping>();

            foreach (var item in _clientsOfThisGame)
            {
                tmpList.Add(new PlayerClientMapping(item.Client_ID, item.Player));
            }

            return tmpList;
        }
    }
}
