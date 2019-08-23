using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using MDC.Exceptions;
using MDC.Gamedata;

namespace MDC.Client
{
    public class ClientProgram
    {
        private int _port_NO;
        private string _server_IP;
        private string _client_ID;
        private TcpClient _server;
        private string _gameSession_ID;
        public string GameSession_ID { get { return _gameSession_ID; } }
        private Boolean _isConnected = false;
        public Boolean IsConnected { get { return _isConnected; } }
        private Boolean _isHost = false;

        /// <summary>
        /// generates a tcp client and networkstream to send data or receive data from the server
        /// Establish a connection to the server and receive the client_ID
        /// </summary>
        public void ConnectToServer()
        {
            ReadConfig();
            Console.WriteLine("Connecting to: " + _server_IP);

            try
            {
                //---create a TCPClient object at the IP and port no.---
                _server = new TcpClient(_server_IP, _port_NO);
                // _server.ReceiveTimeout = 3000; // 3 Seconds
                // _server.SendTimeout = 3000; // 3 Seconds

                //---get the client ID from the server---
                _client_ID = ReceiveStringFromServer();
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.GetType().FullName);
                Console.WriteLine("\t" + e.Message);
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
                _server.Close();
                _client_ID = null;
                _isConnected = false;
            }
            else if (_isHost)
            {
                throw new NotImplementedException();
                //TODO: Wenn Host disconnected, auf Serverseite alle Clients disconnecten.
            }
            else
            {
            }
        }

        /// <summary>
        /// Establish a connection to the game session
        /// </summary>
        /// <param name="sessionID">The SessionID of the game to be joined</param>
        public void ConnectToGame(string sessionID)
        {
            if (_isConnected)
            {
                Console.WriteLine("Connecting to: " + sessionID);
                CommandServerJoinGame command = new CommandServerJoinGame(_client_ID, sessionID);
                SendCommandToServer(command);

                CommandFeedback feedback = EvaluateFeedback();
                if (feedback is CommandFeedbackOK)
                {
                    _gameSession_ID = sessionID;
                }
                else
                {
                    feedback.Execute();
                    throw feedback.FeedbackException;
                }
            }
        }


        /// <summary>
        /// Create a new game session
        /// </summary>
        public void CreateNewGame()
        {
            if (_isConnected)
            {
                CommandServerNewGame command = new CommandServerNewGame(_client_ID);
                SendCommandToServer(command);

                CommandFeedback feedback = EvaluateFeedback();
                if (feedback is CommandFeedbackOK)
                {
                    _gameSession_ID = ReceiveStringFromServer();
                    Console.WriteLine("ID of the new Game: " + _gameSession_ID);
                    _isHost = true;
                }
                else
                {
                    // throw feedback.FeedbackException;
                    feedback.Execute();
                }
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
                if (_gameSession_ID != null)
                {
                    CommandServerStartGame command = new CommandServerStartGame(_client_ID, _gameSession_ID);
                    SendCommandToServer(command);

                    CommandFeedback feedback = EvaluateFeedback();
                    if (feedback is CommandFeedbackOK)
                    {
                    }
                    else
                    {
                        // throw feedback.FeedbackException;
                        feedback.Execute();
                    }
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
        public void CreateNewPlayerForSession(string playerName, CharacterClasses pClass)
        {
            if (_isConnected)
            {
                if (_gameSession_ID != null)
                {
                    CommandServerCreatePlayer command = new CommandServerCreatePlayer(_client_ID, _gameSession_ID, playerName, pClass);
                    SendCommandToServer(command);

                    CommandFeedback feedback = EvaluateFeedback();
                    if (feedback is CommandFeedbackOK) { }
                    else
                    {
                        // throw feedback.FeedbackException;
                        feedback.Execute();
                    }

                }
            }
        }

        /// <summary>
        /// Creates a command to move your character on the playing field.
        /// </summary>
        public void MovePlayer(int x, int y)
        { //TODO: Übergabe der Richtung und Anzahl Schritte
            if (_isConnected)
            {
                if (_gameSession_ID != null)
                {
                    CommandGameMove command = new CommandGameMove(_client_ID, x, y);
                    SendCommandToServer(command);

                    CommandFeedback feedback = EvaluateFeedback();
                    if (feedback is CommandFeedbackOK) { }
                    else
                    {
                        // throw feedback.FeedbackException;
                        feedback.Execute();
                    }
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
                if (_gameSession_ID != null)
                {
                    CommandGameAttack command = new CommandGameAttack(_client_ID, client_ID_From_Enemy);
                    SendCommandToServer(command);

                    CommandFeedback feedback = EvaluateFeedback();
                    if (feedback is CommandFeedbackOK) { }
                    else
                    {
                        // throw feedback.FeedbackException;
                        feedback.Execute();
                    }
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
            NetworkStream nwStream = _server.GetStream();
            byte[] bytesToRead = new byte[_server.ReceiveBufferSize];
            int bytesRead = nwStream.Read(bytesToRead, 0, _server.ReceiveBufferSize);

            return Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
        }

        /// <summary>
        /// Send string to client
        /// </summary>
        /// <param name="server">TcpClient to which data is to be sent.</param>
        /// <param name="data">String you want to send</param>
        private void SendStringToServer(string data)
        {
            NetworkStream nwStream = _server.GetStream();
            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(data);
            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
        }

        /// <summary>
        /// Send Command to server
        /// </summary>
        /// <param name="server">TcpClient to which data is to be sent.</param>
        /// <param name="command">Command you want to send</param>
        private void SendCommandToServer(Command command)
        {
            NetworkStream nwStream = _server.GetStream();
            MemoryStream dataStream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();

            var ms = new MemoryStream();
            formatter.Serialize(ms, command);

            byte[] bytesToSend = ms.ToArray();

            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
            nwStream.Flush();
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

        /// <summary>
        /// Should be called after each command sent to the server: 
        /// Evaluates feedback from the server.
        /// </summary>
        /// <returns>True: If the command was successfully processed by the server. False: If command could not be executed.</returns>
        private CommandFeedback EvaluateFeedback()
        {
            // //TODO: MemoryStream-Konzept einbauen
            // NetworkStream nwStream = _server.GetStream();
            // IFormatter formatter = new BinaryFormatter();

            // // CommandFeedback feedback = (CommandFeedback)formatter.Deserialize(nwStream);
            // return (CommandFeedback)formatter.Deserialize(nwStream);


            NetworkStream nwStream = _server.GetStream();
            MemoryStream dataStream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();

            byte[] bytesToRead = new byte[_server.ReceiveBufferSize];
            int bytesRead = nwStream.Read(bytesToRead, 0, _server.ReceiveBufferSize);

            dataStream.Write(bytesToRead, 0, bytesToRead.Length);
            dataStream.Seek(0, SeekOrigin.Begin);

            try
            {
                var obj = formatter.Deserialize(dataStream);
                Console.WriteLine(obj.GetType());
                if (obj is CommandFeedback)
                {
                    return (CommandFeedback)obj;
                }
            }
            catch (System.Runtime.Serialization.SerializationException e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }
    }
}
