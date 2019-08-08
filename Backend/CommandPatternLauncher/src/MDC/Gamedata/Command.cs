//Command
using System;

namespace MDC.Gamedata
{
    [Serializable]
    public abstract class Command
    {
        public string ClientID { get; set; }
        public bool IsCompleted { set; get; }

        public Command(string clientID)
        {
            this.ClientID = clientID;
        }
        abstract public void Execute();
    }
}