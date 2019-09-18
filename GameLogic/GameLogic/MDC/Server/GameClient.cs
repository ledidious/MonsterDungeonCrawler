using System;
using System.Net.Sockets;
using System.Threading;
using GameLogic.MDC.Gamedata.PlayerType;

namespace GameLogic.MDC.Server
{
    public class GameClient
    {
        public TcpClient TcpClient { get; }
        public Player Player { get; set; }
        public String Client_ID { get; }
        public Boolean IsInGame { get; set; }
        public Boolean IsHost { get; set; }
        private Thread myThread;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:GameLogic.MDC.Server.GameClient"/> class.
        /// </summary>
        /// <param name="client">Client.</param>
        /// <param name="client_ID">Client identifier.</param>
        public GameClient(TcpClient client, String client_ID)
        {
            this.TcpClient = client;
            this.Client_ID = client_ID;
            this.IsInGame = false;
            myThread = System.Threading.Thread.CurrentThread;
        }

        /// <summary>
        /// Kills the thread.
        /// </summary>
        public void killThread()
        {
            myThread.Abort();
            myThread.Join();
        }
    }
}
