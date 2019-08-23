using System;
using System.Net.Sockets;

namespace MDC.Server
{
    public class GameClient
    {
        public TcpClient Client { get; set; }
        public GameClient(TcpClient client)
        {
            this.Client = client;
        }
        
        public String ClientID { get; set; }
    }
}