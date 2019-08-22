//Command
using System;
using MDC.Gamedata.PlayerType;

namespace MDC.Gamedata
{
    [Serializable]
    public abstract class CommandServer : Command
    {
        public CommandServer(string SourceClientID) : base(SourceClientID)
        {
        }
    }

    /// <summary>
    /// Create a new game
    /// </summary>
    [Serializable]
    public class CommandServerNewGame : CommandServer
    {
        public CommandServerNewGame(string SourceClientID) : base(SourceClientID)
        {
        }

        public override void Execute()
        {
            MasterServer.CreateNewGame(SourceClientID);
            this.IsCompleted = true;
        }
    }

    /// <summary>
    /// Join an existing game
    /// </summary>
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

    /// <summary>
    /// Start the round of an existing game
    /// </summary>
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

    /// <summary>
    /// Cancel a current round of play
    /// </summary>
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

    /// <summary>
    /// Create a new player for an existing game session.
    /// </summary>
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