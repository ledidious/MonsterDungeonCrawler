using System;
using GameLogic.MDC.Gamedata.PlayerType;
using GameLogic.MDC.Server;

namespace GameLogic.MDC.Gamedata
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
        string levelFileName;

        public CommandServerNewGame(string SourceClientID, string levelFileName) : base(SourceClientID)
        {
            this.levelFileName = levelFileName;
        }

        public override void Execute()
        {
            MasterServer.CreateNewGame(SourceClientID, levelFileName);
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
            MasterServer.ConnectClientToGame(SourceClientID, SessionID);
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
            MasterServer.StartGame(SourceClientID, SessionID);
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
        private string _session_ID;
        private CharacterClass _characterClass;
        private Player player;

        public CommandServerCreatePlayer(string SourceClientID, string session_ID, string playerName, CharacterClass characterClass) : base(SourceClientID)
        {
            this._playerName = playerName;
            this._session_ID = session_ID;
            this._characterClass = characterClass;
        }

        public override void Execute()
        {
            MasterServer.CreateNewPlayerForSession(SourceClientID, _session_ID, _playerName, _characterClass);
            this.IsCompleted = true;
        }
    }

    /// <summary>
    /// Cancel a current round of play
    /// </summary>
    [Serializable]
    public class CommandServerGetUpdatePackForLobby : CommandServer
    {
        string session_ID;
        public CommandServerGetUpdatePackForLobby(string SourceClientID, string session_ID) : base(SourceClientID)
        {
            this.session_ID = session_ID;
        }

        public override void Execute()
        {
            MasterServer.SendUpdatePackForLobby(SourceClientID, session_ID);
        }
    }
}
