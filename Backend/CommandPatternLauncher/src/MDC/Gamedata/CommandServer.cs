//Command
using System;
using MDC.Server;

namespace MDC.Gamedata
{
    [Serializable]
    public abstract class CommandServer : Command
    {
        // public Game TargetGame { get; set; }
        // public MasterServer TargetServer {get; set;}
        public CommandServer(string SourceClientID) : base(SourceClientID)
        {
        }
    }

    [Serializable]
    public class CommandServerNewGame : CommandServer
    {
        public CommandServerNewGame(string SourceClientID) : base(SourceClientID)
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
    public class CommandServerJoinGame : CommandServer
    {
        public CommandServerJoinGame(string SourceClientID, string sessionID, string playerName) : base(SourceClientID)
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
    public class CommandServerStartGame : CommandServer
    {
        public CommandServerStartGame(string SourceClientID, string sessionID) : base(SourceClientID)
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
    public class CommandServerAbortGame : CommandServer
    {
        public CommandServerAbortGame(string SourceClientID) : base(SourceClientID)
        {
        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}