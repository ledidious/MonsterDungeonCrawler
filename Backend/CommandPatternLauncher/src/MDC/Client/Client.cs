using System;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using MDC.Gamedata;

namespace MDC.Client
{
    public class ClientProgram
    {
        const int PORT_NO = 5000;
        const string SERVER_IP = "127.0.0.1";

        static string CLIENT_ID;

        /// <summary>
        /// generates a tcp client and networkstream to send data or receive data from the server
        /// using formatter (serialization) for sending player and move objects as a binary
        /// </summary>
        public static void StartClient()
        {
            Console.WriteLine("Connecting to: " + SERVER_IP);

            //---create a TCPClient object at the IP and port no.---
            TcpClient server = new TcpClient(SERVER_IP, PORT_NO);

            //---send the player name to the server---
            SendStringToServer(server, "Vegeta");

            //---get the client ID from the server---
            CLIENT_ID = ReceiveStringFromServer(server);

            //---create a command for the player of this client---
            CommandMove command = new CommandMove(CLIENT_ID, 5);
            SendCommandToServer(server, command);

            server.Close();
        }

        /// <summary>
        /// Receive string from server
        /// </summary>
        /// <param name="server">TcpClient from which data is to be received.</param>
        /// <returns>Returns the received string</returns>
        private static string ReceiveStringFromServer(TcpClient server)
        {
            NetworkStream nwStream = server.GetStream();
            byte[] bytesToRead = new byte[server.ReceiveBufferSize];
            int bytesRead = nwStream.Read(bytesToRead, 0, server.ReceiveBufferSize);

            return Encoding.ASCII.GetString(bytesToRead, 0, bytesRead); ;
        }

        /// <summary>
        /// Send string to client
        /// </summary>
        /// <param name="server">TcpClient to which data is to be sent.</param>
        /// <param name="data">String you want to send</param>
        private static void SendStringToServer(TcpClient server, string data)
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
        private static void SendCommandToServer(TcpClient server, Command command)
        {
            NetworkStream nwStream = server.GetStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(nwStream, command);
        }
    }
}
