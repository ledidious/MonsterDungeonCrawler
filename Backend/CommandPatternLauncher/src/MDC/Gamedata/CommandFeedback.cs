//Command
using System;
using MDC.Server;

namespace MDC.Gamedata
{
    [Serializable]
    public abstract class CommandFeedback : Command
    {
        public CommandFeedback(string SourceClientID) : base(SourceClientID)
        {
        }
    }

    [Serializable]
    public class CommandFeedbackNotInRange : CommandFeedback
    {
        public CommandFeedbackNotInRange(string SourceClientID) : base(SourceClientID)
        {
        }

        public override void Execute()
        {
            MasterServer.CreateNewGame(SourceClientID);
            // throw new NotImplementedException();
        }
    }

    public class CommandFeedbackPathIsBlocked : CommandFeedback
    {
        public CommandFeedbackPathIsBlocked(string SourceClientID) : base(SourceClientID)
        {
        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}