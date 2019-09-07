using System;
using System.Net.Sockets;
using System.Threading;
using MDC.Gamedata.PlayerType;

namespace MDC.Server
{
    public class GameClient
    {
        public TcpClient TcpClient { get; }
        public Player Player { get; set; }
        public String Client_ID { get; }
        public Boolean IsInGame { get; set; }
        public Boolean IsHost { get; set; }
        private Thread myThread;

        public GameClient(TcpClient client, String client_ID)
        {
            this.TcpClient = client;
            this.Client_ID = client_ID;
            this.IsInGame = false;
            myThread = System.Threading.Thread.CurrentThread;
        }

        public void killThread()
        {
            myThread.Abort();
            myThread.Join();
        }
    }
}