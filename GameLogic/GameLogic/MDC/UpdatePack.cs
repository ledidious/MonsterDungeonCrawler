using System;
using System.Collections.Generic;
using GameLogic.MDC.Gamedata.PlayerType;
using GameLogic.MDC.Gamedata.LevelContent;

namespace GameLogic.MDC
{
    [Serializable]
    public class UpdatePack
    {
        public int NumberOfPlayedRounds { get; }
        public string ActiveClientID { get; }
        public List<PlayerClientMapping> PlayerList { get; }
        public Field[,] PlayingField { get; }
        public List<Field> TrapList { get; }
        public string activeScene { get; }
        public int hash { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:GameLogic.MDC.UpdatePack"/> class.
        /// </summary>
        /// <param name="playerList">Player list.</param>
        /// <param name="playingField">Playing field.</param>
        /// <param name="trapList">Trap list.</param>
        /// <param name="activeScene">Active scene.</param>
        public UpdatePack(int numberOfPlayedRounds, string activeClientID , List<PlayerClientMapping> playerList, Field[,] playingField, List<Field> trapList, string activeScene)
        {
            this.NumberOfPlayedRounds = numberOfPlayedRounds;
            this.ActiveClientID = activeClientID;
            this.PlayerList = playerList;
            this.PlayingField = playingField;
            this.TrapList = trapList;
            this.activeScene = activeScene;
            this.hash = this.GetHashCode();
        }
    }

    /// <summary>
    /// Player client mapping.
    /// </summary>
    [Serializable]
    public class PlayerClientMapping
    {
        public string Client_ID { get; }
        public Player Player { get; }

        public PlayerClientMapping(string client_ID, Player player)
        {
            this.Client_ID = client_ID;
            this.Player = player;
        }
    }
}
