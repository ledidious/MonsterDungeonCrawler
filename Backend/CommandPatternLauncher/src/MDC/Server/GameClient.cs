using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using MDC.Gamedata;
using MDC.Gamedata.PlayerType;

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