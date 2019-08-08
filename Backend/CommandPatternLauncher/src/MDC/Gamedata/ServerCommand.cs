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
        public ServerCommand(string clientID) : base(clientID)
        {
        }
    }

    [Serializable]
    public class CommandNewGame : ServerCommand
    {
        public CommandNewGame(string clientID) : base(clientID)
        {
        }

        // public String PlayerName {get; set;}
        public override void Execute()
        {
            MasterServer.CreateNewGame(ClientID);
            // throw new NotImplementedException();
        }
    }

    [Serializable]
    public class CommandJoinGame : ServerCommand
    {
        public CommandJoinGame(string clientID, string sessionID, string playerName) : base(clientID)
        {
            SessionID = sessionID;
            PlayerName = playerName;
        }

        public String PlayerName { get; set; }
        public String SessionID { get; set; }
        public override void Execute()
        {
            MasterServer.ConnectToGame(ClientID, SessionID , PlayerName);
            // throw new NotImplementedException();
        }
    }

    [Serializable]
    public class CommandStartGame : ServerCommand
    {
        public CommandStartGame(string clientID, string sessionID) : base(clientID)
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
        public CommandKillGame(string clientID) : base(clientID)
        {
        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}