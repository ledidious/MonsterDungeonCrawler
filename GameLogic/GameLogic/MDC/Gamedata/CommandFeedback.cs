using System;

namespace GameLogic.MDC.Gamedata
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
}
