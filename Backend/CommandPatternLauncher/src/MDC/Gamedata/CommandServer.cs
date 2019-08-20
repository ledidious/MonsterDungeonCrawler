//Command
using System;
using MDC.Gamedata.PlayerType;
using MDC.Server;

namespace MDC.Gamedata
{
    // public enum CharacterClasses
    // {
    //     MeleeFighter,
    //     RangeFighter
    // }

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
            this.IsCompleted = true;
        }
    }

    [Serializable]
    public class CommandServerJoinGame : CommandServer
    {
        public String SessionID { get; }

        public CommandServerJoinGame(string SourceClientID, string sessionID) : base(SourceClientID)
        {
            SessionID = sessionID;
        }

        public override void Execute()
        {
            MasterServer.ConnectToGame(SourceClientID, SessionID);
            this.IsCompleted = true;
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
            this.IsCompleted = true;
            MasterServer.StartGame(SessionID);
            // throw new NotImplementedException();
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

    [Serializable]
    public class CommandServerCreatePlayer : CommandServer
    {
        private string _playerName;
        private string _sessionID;
        private Player player;

        public CommandServerCreatePlayer(string SourceClientID, string sessionID, string playerName, CharacterClasses characterClass) : base(SourceClientID)
        {
            this._playerName = playerName;
            this._sessionID = sessionID;


            switch (characterClass)
            {
                case CharacterClasses.MeleeFighter:
                    player = new Hero(playerName, new MeleeFighter(), 0, 0);
                    break;
                case CharacterClasses.RangeFighter:
                    player = new Hero(playerName, new RangeFighter(), 0, 0);
                    break;
                default:
                    break;
            }
        }



        public override void Execute()
        {
            MasterServer.CreateNewPlayerForSession(SourceClientID, _sessionID, player);
            this.IsCompleted = true;
        }
    }
}