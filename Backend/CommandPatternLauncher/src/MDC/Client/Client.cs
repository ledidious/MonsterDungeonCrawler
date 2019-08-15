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
        private int PORT_NO;
        private string SERVER_IP;
        protected string client_ID;
        private TcpClient server;
        protected string gameSession_ID;
        private Boolean isConnected = false;



        /// <summary>
        /// generates a tcp client and networkstream to send data or receive data from the server
        /// using formatter (serialization) for sending player and move objects as a binary
        /// </summary>
        public void ConnectToServer()
        {
            server.ReceiveTimeout = 3000; // 3 Seconds
            server.SendTimeout = 3000; // 3 Seconds
            ReadConfig();
            Console.WriteLine("Connecting to: " + SERVER_IP);

            //---create a TCPClient object at the IP and port no.---
            server = new TcpClient(SERVER_IP, PORT_NO);

            //---get the client ID from the server---
            client_ID = ReceiveStringFromServer();
            isConnected = true;
        }

        public void DisconnectFromServer()
        {
            if (isConnected)
            {
                server.Close();
                isConnected = false;
            }
        }

        private void ReadConfig()
        {
            var data = new Dictionary<string, string>();
            foreach (var row in File.ReadAllLines("game.config"))
                data.Add(row.Split('=')[0], string.Join("=", row.Split('=').Skip(1).ToArray()));

            SERVER_IP = data["ServerIP"];
            PORT_NO = Convert.ToInt32(data["PortNo"]);
        }

        public void ConnectToGame(string sessionID)
        {
            if (isConnected)
            {
                Console.WriteLine("Connecting to: " + sessionID);
                CommandServerJoinGame command = new CommandServerJoinGame(client_ID, sessionID, "Luffy");
                SendCommandToServer(command);

                gameSession_ID = ReceiveStringFromServer();
            }
        }

        public void CreateNewGame()
        {
            if (isConnected)
            {
                CommandServerNewGame command = new CommandServerNewGame(client_ID);
                SendCommandToServer(command);

                gameSession_ID = ReceiveStringFromServer();
                Console.WriteLine("ID of the new Game: " + gameSession_ID);
            }
        }

        public void StartCreatedGame()
        {
            if (isConnected)
            {
                if (gameSession_ID != null)
                {
                    CommandServerStartGame command = new CommandServerStartGame(client_ID, gameSession_ID);
                    SendCommandToServer(command);
                }
                else
                {
                    throw new NullReferenceException();
                }

                getFeedbackFromServer();
            }
        }

        /// <summary>
        /// Receive string from server
        /// </summary>
        /// <param name="server">TcpClient from which data is to be received.</param>
        /// <returns>Returns the received string</returns>
        private string ReceiveStringFromServer()
        {
            NetworkStream nwStream = server.GetStream();
            byte[] bytesToRead = new byte[server.ReceiveBufferSize];
            int bytesRead = nwStream.Read(bytesToRead, 0, server.ReceiveBufferSize);

            return Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
        }

        /// <summary>
        /// Send string to client
        /// </summary>
        /// <param name="server">TcpClient to which data is to be sent.</param>
        /// <param name="data">String you want to send</param>
        private void SendStringToServer(string data)
        {
            NetworkStream nwStream = server.GetStream();
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
            NetworkStream nwStream = server.GetStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(nwStream, command);
        }

        private Boolean getFeedbackFromServer()
        {
            return false;
        }

    }
}
