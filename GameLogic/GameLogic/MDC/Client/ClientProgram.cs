using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using GameLogic.MDC.Gamedata;

namespace GameLogic.MDC.Client
{
    public class ClientProgram
    {
        private int _port_NO;
        private string _server_IP;
        private string _client_ID;
        public string Client_ID { get { return _client_ID; } }
        private TcpClient _masterServer;
        private string _gameSession_ID;
        public string GameSession_ID { get { return _gameSession_ID; } }
        private Boolean _isConnected = false;
        public Boolean IsConnected { get { return _isConnected; } }
        private Boolean _isHost = false;
        public Boolean IsHost { get { return _isHost; } }

        public enum Status
        {
            Busy,
            Waiting,
            Spectator
        }

        private Status _currentStatus;
        public Status CurrentStatus { get { return (_currentStatus); } }
        private UpdatePack _update;
        public UpdatePack Update { get { return _update; } }

        public ClientProgram()
        {
            this._currentStatus = Status.Busy;
        }

        /// <summary>
        /// generates a tcp client and networkstream to send data or receive data from the server
        /// Establish a connection to the server and receive the client_ID
        /// </summary>
        public void ConnectToServer()
        {
            ReadConfig();
            // Console.WriteLine("Connecting to: " + _server_IP);

            try
            {
                //---create a TCPClient object at the IP and port no.---
                _masterServer = new TcpClient(_server_IP, _port_NO);
                //_server.ReceiveTimeout = 3000; // 3 Seconds TODO: Wieder einbauen
                //_server.SendTimeout = 3000; // 3 Seconds TODO: Wieder einbauen
                _masterServer.SendBufferSize = 524288;
                _masterServer.ReceiveBufferSize = 524288;

                //---get the client ID from the server---
                _client_ID = ReceiveStringFromServer();
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.GetType().FullName);
                Console.WriteLine("\t" + e.Message);
                throw new CannotConnectToServerException();
            }

            if (_client_ID != null)
            {
                _isConnected = true;
            }
        }

        /// <summary>
        /// Disconnect Client from Server and delete client_ID
        /// </summary>
        public void DisconnectFromServer()
        {
            if (_isConnected && _isHost == false)
            {
                _masterServer.Close();
                _client_ID = null;
                _isConnected = false;
            }
            else if (_isConnected && _isHost)
            {
                throw new NotImplementedException();
                //TODO: Wenn Host disconnected, auf Serverseite alle Clients disconnecten.
            }
            else { throw new ClientIsNotConnectedToServerException(); }
        }

        /// <summary>
        /// Establish a connection to the game session
        /// </summary>
        /// <param name="session_ID">The SessionID of the game to be joined</param>
        public void ConnectToGame(string session_ID)
        {
            if (_isConnected)
            {
                Console.WriteLine("Connecting to: " + session_ID);
                CommandServerJoinGame command = new CommandServerJoinGame(_client_ID, session_ID);
                SendCommandToServer(command);

                CommandFeedback feedback = EvaluateFeedback();
                if (feedback is CommandFeedbackOK)
                {
                    _gameSession_ID = session_ID;
                }
                else if (feedback is CommandFeedbackGameException) { throw ((CommandFeedbackGameException)feedback).GameException; }
                else { throw new CommandNotRecognizedException(); }
            }
        }

        /// <summary>
        /// Create a new game session
        /// </summary>
        public void CreateNewGame(string levelFileName)
        {
            if (_isConnected)
            {
                CommandServerNewGame command = new CommandServerNewGame(_client_ID, levelFileName);
                SendCommandToServer(command);

                CommandFeedback feedback = EvaluateFeedback();
                if (feedback is CommandFeedbackOK)
                {
                    _gameSession_ID = ReceiveStringFromServer();
                    Console.WriteLine("ID of the new Game: " + _gameSession_ID);
                    _isHost = true;
                }
                else if (feedback is CommandFeedbackGameException) { throw ((CommandFeedbackGameException)feedback).GameException; }
                else { throw new CommandNotRecognizedException(); }
            }
            else
            {
                throw new ClientIsNotConnectedToServerException();
            }
        }

        /// <summary>
        /// Start the current game. 
        /// Can only be executed by the host and only when all players are connected.
        /// </summary>
        public void StartCreatedGame()
        {
            if (_isConnected)
            {
                if (_gameSession_ID != null && _isHost)
                {
                    CommandServerStartGame command = new CommandServerStartGame(_client_ID, _gameSession_ID);
                    SendCommandToServer(command);

                    CommandFeedback feedback = EvaluateFeedback();
                    if (feedback is CommandFeedbackOK) { }
                    else if (feedback is CommandFeedbackGameException) { throw ((CommandFeedbackGameException)feedback).GameException; }
                    else { throw new CommandNotRecognizedException(); }
                }
            }
            else
            {
                throw new ClientIsNotConnectedToServerException();
            }
        }

        /// <summary>
        /// Creates a new player for the client in the current session
        /// </summary>
        /// <param name="playerName">Name of the character</param>
        /// <param name="pClass">The class of character of the character</param>
        public void CreateNewPlayerForSession(string playerName, CharacterClass pClass)
        {
            if (_isConnected)
            {
                if (_gameSession_ID != null)
                {
                    CommandServerCreatePlayer command = new CommandServerCreatePlayer(_client_ID, _gameSession_ID, playerName, pClass);
                    SendCommandToServer(command);

                    CommandFeedback feedback = EvaluateFeedback();
                    if (feedback is CommandFeedbackOK) { }
                    else if (feedback is CommandFeedbackEndOfTurn)
                    {
                        Thread updateThread = new Thread(new ThreadStart(() => WaitForNextTurn()));
                        updateThread.Start();
                    }
                    else if (feedback is CommandFeedbackGameException) { throw ((CommandFeedbackGameException)feedback).GameException; }
                    else { throw new CommandNotRecognizedException(); }
                }
            }
            else
            {
                throw new ClientIsNotConnectedToServerException();
            }
        }

        /// <summary>
        /// Creates a command to move your character on the playing field.
        /// </summary>
        public void MovePlayer(int x, int y)
        { //TODO: Übergabe der Richtung und Anzahl Schritte
            if (_isConnected)
            {
                if (_gameSession_ID != null && _currentStatus == Status.Busy)
                {
                    CommandGameMove command = new CommandGameMove(_client_ID, x, y);
                    SendCommandToServer(command);

                    CommandFeedback feedback = EvaluateFeedback();
                    if (feedback is CommandFeedbackOK) { }
                    else if (feedback is CommandFeedbackEndOfTurn)
                    {
                        Thread updateThread = new Thread(new ThreadStart(() => WaitForNextTurn()));
                        updateThread.Start();
                    }
                    else if (feedback is CommandFeedbackGameException) { throw ((CommandFeedbackGameException)feedback).GameException; }
                    else { throw new CommandNotRecognizedException(); }
                }
            }
        }

        /// <summary>
        /// Creates a command that lets your character attack another character
        /// </summary>
        /// <param name="client_ID_From_Enemy">The ClientID of the opponent</param>
        public void AttackEnemy(string client_ID_From_Enemy)
        {
            if (_isConnected)
            {
                if (_gameSession_ID != null && _currentStatus == Status.Busy)
                {
                    CommandGameAttack command = new CommandGameAttack(_client_ID, client_ID_From_Enemy);
                    SendCommandToServer(command);

                    CommandFeedback feedback = EvaluateFeedback();
                    if (feedback is CommandFeedbackOK) { }
                    else if (feedback is CommandFeedbackEndOfTurn)
                    {
                        Thread updateThread = new Thread(new ThreadStart(() => WaitForNextTurn()));
                        updateThread.Start();
                    }
                    else if (feedback is CommandFeedbackGameException) { throw ((CommandFeedbackGameException)feedback).GameException; }
                    else { throw new CommandNotRecognizedException(); }
                }
            }
        }

        /// <summary>
        /// Ends the player's turn earlier than normal
        /// </summary>
        public void EndTurn()
        {
            if (_isConnected)
            {
                if (_gameSession_ID != null && _currentStatus == Status.Busy)
                {
                    CommandGameEndTurn command = new CommandGameEndTurn(_client_ID);
                    SendCommandToServer(command);

                    CommandFeedback feedback = EvaluateFeedback();
                    if (feedback is CommandFeedbackOK)
                    {
                        Thread updateThread = new Thread(new ThreadStart(() => WaitForNextTurn()));
                        updateThread.Start();
                    }
                    else if (feedback is CommandFeedbackEndOfTurn) { Thread updateThread = new Thread(new ThreadStart(() => WaitForNextTurn())); updateThread.Start(); }
                    else if (feedback is CommandFeedbackGameException) { throw ((CommandFeedbackGameException)feedback).GameException; }
                    else { throw new CommandNotRecognizedException(); }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void GetUpdatePackForLobby()
        {
            if (_isConnected)
            {
                if (_gameSession_ID != null && _currentStatus == Status.Busy)
                {
                    CommandServerGetUpdatePackForLobby command = new CommandServerGetUpdatePackForLobby(_client_ID, _gameSession_ID);
                    SendCommandToServer(command);

                    CommandFeedback feedback = EvaluateFeedback();
                    if (feedback is CommandFeedbackUpdatePack)
                    {
                        _update = ((CommandFeedbackUpdatePack)feedback).Update;
                    }
                    else if (feedback is CommandFeedbackGameException) { throw ((CommandFeedbackGameException)feedback).GameException; }
                    else
                    {
                        throw new CommandNotRecognizedException();
                    }
                }
            }
        }

        /// <summary>
        /// Get an update of the playing field and wait for the call for your next move.
        /// It is best to encapsulate it in its own thread so that the client does not get stuck.
        /// </summary>
        private void WaitForNextTurn()
        {
            _currentStatus = Status.Waiting;
            CommandFeedback feedback;

            do
            {
                Console.WriteLine("UPDATE...");
                feedback = EvaluateFeedback();

                if (feedback is CommandFeedbackUpdatePack)
                {
                    if (((CommandFeedbackUpdatePack)feedback).PlayerAlive == false && _currentStatus != Status.Spectator)
                    {
                        _currentStatus = Status.Spectator;
                        _update = ((CommandFeedbackUpdatePack)feedback).Update;
                    }
                    else
                    {
                        _update = ((CommandFeedbackUpdatePack)feedback).Update;
                    }

                }
            } while (feedback is CommandFeedbackUpdatePack);

            if (feedback is CommandFeedbackYourTurn)
            {
                if (_currentStatus != Status.Spectator)
                {
                    _currentStatus = Status.Busy;
                    //System.Threading.Thread.CurrentThread.Abort();
                    //TODO: Könnte später Probleme geben, weil Thread nicht gekillt wird. Sollte aber durch _currentStatus geregelt werden.
                }
            }
        }

        /// <summary>
        /// Receive string from server
        /// </summary>
        /// <param name="server">TcpClient from which data is to be received.</param>
        /// <returns>Returns the received string</returns>
        private string ReceiveStringFromServer()
        {
            NetworkStream nwStream = _masterServer.GetStream();
            Byte[] bytesToRead = new byte[_masterServer.ReceiveBufferSize];
            Int32 bytesRead = nwStream.Read(bytesToRead, 0, bytesToRead.Length);

            return Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
        }

        /// <summary>
        /// Send string to client
        /// </summary>
        /// <param name="server">TcpClient to which data is to be sent.</param>
        /// <param name="data">String you want to send</param>
        private void SendStringToServer(string data)
        {
            NetworkStream nwStream = _masterServer.GetStream();
            Byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(data);
            nwStream.Write(bytesToSend, 0, bytesToSend.Length);

        }

        /// <summary>
        /// Send Command to server
        /// </summary>
        /// <param name="server">TcpClient to which data is to be sent.</param>
        /// <param name="command">Command you want to send</param>
        private void SendCommandToServer(Command command)
        {
            NetworkStream nwStream = _masterServer.GetStream();
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

             nwStream.Write(bytesToSend, 0, bytesToSend.Length); */
            formatter.Serialize(nwStream, command);
            nwStream.Flush();
        }

        /// <summary>
        /// Should be called after each command sent to the server: 
        /// Evaluates feedback from the server.
        /// </summary>
        /// <returns>True: If the command was successfully processed by the server. False: If command could not be executed.</returns>
        private CommandFeedback EvaluateFeedback()
        {
            NetworkStream nwStream = _masterServer.GetStream();
            //MemoryStream dataStream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();

            /* byte[] bytesToRead = new byte[_masterServer.ReceiveBufferSize];
             nwStream.Read(bytesToRead, 0, _masterServer.ReceiveBufferSize);

             dataStream.Write(bytesToRead, 0, bytesToRead.Length);
             dataStream.Seek(0, SeekOrigin.Begin); */

            try
            {
                var obj = formatter.Deserialize(nwStream);
                nwStream.Flush();
                //Console.WriteLine("CLIENT: " + obj.GetType());
                if (obj is CommandFeedback)
                {
                    return (CommandFeedback)obj;
                }
            }
            catch (System.Runtime.Serialization.SerializationException e)
            {
                throw e;
                //Console.WriteLine(e.Message);
            }

            nwStream.Flush();
            return null;
        }

        /// <summary>
        /// Read the configuration from the config-file and store the value in the corresponding variables.
        /// </summary>
        private void ReadConfig()
        {
            var data = new Dictionary<string, string>();
            foreach (var row in File.ReadAllLines("game.config"))
                data.Add(row.Split('=')[0], string.Join("=", row.Split('=').Skip(1).ToArray()));

            _server_IP = data["ServerIP"];
            _port_NO = Convert.ToInt32(data["PortNo"]);
        }
    }
}
