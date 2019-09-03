//Command
using System;
using System.Runtime.Serialization;
using MDC.Exceptions;
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
    public class CommandFeedbackEndOfTurn : CommandFeedback
    {
        public CommandFeedbackEndOfTurn(string SourceClientID) : base(SourceClientID)
        {
            IsCompleted = true;
        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }

    [Serializable]
    public class CommandFeedbackYourTurn : CommandFeedback
    {
        public CommandFeedbackYourTurn(string SourceClientID) : base(SourceClientID)
        {
            IsCompleted = true;
        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }

    [Serializable]
    public class CommandFeedbackUpdatePack : CommandFeedback
    {
        public Boolean PlayerAlive { get; }

        public UpdatePack Update { get; }

        public CommandFeedbackUpdatePack(string SourceClientID, Boolean playerAlive, UpdatePack update) : base(SourceClientID)
        {
            this.PlayerAlive = playerAlive;
            this.Update = update;

            IsCompleted = true;
            // throw new NotImplementedException();
        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }

    [Serializable]
    public class CommandFeedbackGameException : CommandFeedback
    {
        public Exception GameException { get; set; }

        public CommandFeedbackGameException(string SourceClientID, Exception e) : base(SourceClientID)
        {
            IsCompleted = false;
            this.GameException = e;
        }

        public override void Execute()
        {
            IsCompleted = true;
        }
    }
    /* 
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
        } */


}