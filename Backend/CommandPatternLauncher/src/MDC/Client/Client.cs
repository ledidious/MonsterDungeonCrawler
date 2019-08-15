using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
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

        /// <summary>
        /// generates a tcp client and networkstream to send data or receive data from the server
        /// Establish a connection to the server and receive the client_ID
        /// </summary>
        public void ConnectToServer()
        {
            ReadConfig();
            Console.WriteLine("Connecting to: " + _server_IP);

            //---create a TCPClient object at the IP and port no.---
            _server = new TcpClient(_server_IP, _port_NO);
            _server.ReceiveTimeout = 3000; // 3 Seconds
            _server.SendTimeout = 3000; // 3 Seconds

            //---get the client ID from the server---
            _client_ID = ReceiveStringFromServer();

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
            if (_isConnected)
            {
                _server.Close();
                _client_ID = null;
                _isConnected = false;
            }
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
        /// Establish a connection to the game session
        /// </summary>
        /// <param name="sessionID"></param>
        public void ConnectToGame(string sessionID)
        {
            if (_isConnected)
            {
                Console.WriteLine("Connecting to: " + sessionID);
                CommandServerJoinGame command = new CommandServerJoinGame(_client_ID, sessionID, "Luffy");
                SendCommandToServer(command);

                if (EvaluateFeedback())
                {
                    _gameSession_ID = ReceiveStringFromServer();
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

                if (EvaluateFeedback())
                {
                    _gameSession_ID = ReceiveStringFromServer();
                    Console.WriteLine("ID of the new Game: " + _gameSession_ID);
                }

                // CommandFeedback feedback = EvaluateFeedback();
                // if (feedback is CommandFeedbackActionExecutedSuccessfully)
                // {
                //     gameSession_ID = ReceiveStringFromServer();
                //     Console.WriteLine("ID of the new Game: " + gameSession_ID);
                // }
                // else
                // {
                //     Console.WriteLine(feedback.GetType());
                // }
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

                    if (EvaluateFeedback())
                        Console.WriteLine("Command executed!");
                    else
                        throw new NotImplementedException();
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        public void CreateNewPlayerForSession(string playerName)
        {
            if (_isConnected)
            {
                if (_gameSession_ID != null)
                {
                    
                }
            }
        }

        public void MovePlayer(int moveAmount)
        {
            if (_isConnected)
            {
                if (_gameSession_ID != null)
                {
                    CommandGameMove command = new CommandGameMove(_client_ID, moveAmount);
                    SendCommandToServer(command);

                    if (EvaluateFeedback())
                    {
                        Console.WriteLine("Command executed!");
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                }
            }
        }

        public void AttackEnemy(string client_ID_From_Enemy)
        {
            if (_isConnected)
            {
                if (_gameSession_ID != null)
                {
                    CommandGameAttack command = new CommandGameAttack(_client_ID, client_ID_From_Enemy);
                    SendCommandToServer(command);

                    if (EvaluateFeedback())
                    {
                        Console.WriteLine("Command executed!");
                    }
                    else
                    {
                        throw new NotImplementedException();
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
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(nwStream, command);
        }

        private Boolean EvaluateFeedback()
        {
            NetworkStream nwStream = _server.GetStream();
            IFormatter formatter = new BinaryFormatter();

            CommandFeedback feedback = (CommandFeedback)formatter.Deserialize(nwStream);

            if (feedback is CommandFeedbackActionExecutedSuccessfully)
            {
                return true;
            }
            else
            {
                Console.WriteLine(feedback.GetType());
                return false;
            }


        }

        /* private CommandFeedback EvaluateFeedback()
        {
            NetworkStream nwStream = server.GetStream();
            IFormatter formatter = new BinaryFormatter();

            return (CommandFeedback)formatter.Deserialize(nwStream);
        }*/
    }
}
