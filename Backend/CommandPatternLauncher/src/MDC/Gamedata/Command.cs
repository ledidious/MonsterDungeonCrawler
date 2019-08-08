//Command
using System;
using MDC.Gamedata.PlayerType;

namespace MDC.Gamedata
{
    [Serializable]
    public abstract class Command
    {
        public string SourceClientID { get; set; }
        public bool IsCompleted { set; get; }


        public Command(string sourceClientID)
        {
            this.SourceClientID = sourceClientID;
        }
        abstract public void Execute();
    }
}