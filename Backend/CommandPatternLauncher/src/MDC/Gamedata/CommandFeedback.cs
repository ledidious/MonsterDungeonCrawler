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
    public class CommandFeedbackActionExecutedSuccessfully : CommandFeedback
    {
        public CommandFeedbackActionExecutedSuccessfully(string SourceClientID) : base(SourceClientID)
        {
        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }

    [Serializable]
    public class CommandFeedbackEnemyNotInRange : CommandFeedback
    {
        public CommandFeedbackEnemyNotInRange(string SourceClientID) : base(SourceClientID)
        {
        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }

    [Serializable]
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

    [Serializable]
    public class CommandFeedbackCannotAttackThisObject : CommandFeedback
    {
        public CommandFeedbackCannotAttackThisObject(string SourceClientID) : base(SourceClientID)
        {
        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}