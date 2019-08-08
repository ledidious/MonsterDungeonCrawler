//Command
using System;
using MDC.Server;

namespace MDC.Gamedata
{
    [Serializable]
    public abstract class ServerCommand : Command
    {
        // public Game TargetGame { get; set; }
        // public MasterServer TargetServer {get; set;}
        public ServerCommand(string SourceClientID) : base(SourceClientID)
        {
        }
    }

    [Serializable]
    public class CommandNewGame : ServerCommand
    {
        public CommandNewGame(string SourceClientID) : base(SourceClientID)
        {
        }

        // public String PlayerName {get; set;}
        public override void Execute()
        {
            MasterServer.CreateNewGame(SourceClientID);
            // throw new NotImplementedException();
        }
    }

    [Serializable]
    public class CommandJoinGame : ServerCommand
    {
        public CommandJoinGame(string SourceClientID, string sessionID, string playerName) : base(SourceClientID)
        {
            SessionID = sessionID;
            PlayerName = playerName;
        }

        public String PlayerName { get; set; }
        public String SessionID { get; set; }
        public override void Execute()
        {
            MasterServer.ConnectToGame(SourceClientID, SessionID , PlayerName);
            // throw new NotImplementedException();
        }
    }

    [Serializable]
    public class CommandStartGame : ServerCommand
    {
        public CommandStartGame(string SourceClientID, string sessionID) : base(SourceClientID)
        {
            SessionID = sessionID;
        }

        public String SessionID { get; set; }

        public override void Execute()
        {
            MasterServer.StartGame(SessionID);
            throw new NotImplementedException();
        }
    }

    [Serializable]
    public class CommandKillGame : ServerCommand
    {
        public CommandKillGame(string SourceClientID) : base(SourceClientID)
        {
        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}