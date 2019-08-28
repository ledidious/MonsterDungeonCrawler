//Command
using System;
using MDC.Exceptions;
using MDC.Server;

namespace MDC.Gamedata
{
    [Serializable]
    public abstract class CommandFeedback : Command
    {
        public Exception FeedbackException { get; set; }

        public CommandFeedback(string SourceClientID) : base(SourceClientID)
        {
        }
    }

    [Serializable]
    public class CommandFeedbackOK : CommandFeedback
    {
        public CommandFeedbackOK(string SourceClientID) : base(SourceClientID)
        {
            IsCompleted = true;
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
            IsCompleted = true;
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
            IsCompleted = true;
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
            IsCompleted = true;
        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }

    [Serializable]
    public class CommandFeedbackSessionIdIsInvalid : CommandFeedback
    {
        public CommandFeedbackSessionIdIsInvalid(string SourceClientID) : base(SourceClientID)
        {
            IsCompleted = false;
        }

        public override void Execute()
        {
            FeedbackException = new SessionIdIsInvalidException();
            IsCompleted = true;
            // throw new SessionIdIsInvalidException();
        }
    }
}