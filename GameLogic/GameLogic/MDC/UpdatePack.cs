using System;
using System.Collections.Generic;
using GameLogic.MDC.Gamedata.PlayerType;
using GameLogic.MDC.Gamedata.LevelContent;

namespace GameLogic.MDC
{
    [Serializable]
    public class UpdatePack
    {
        public List<Player> PlayerList { get; }
        public Field[,] PlayingField { get; }
        public List<Field> TrapList { get; }
        public int hash { get; }

        public UpdatePack(List<Player> playerList, Field[,] playingField, List<Field> trapList)
        {
            this.PlayerList = playerList;
            this.PlayingField = playingField;
            this.TrapList = trapList;
            this.hash = this.GetHashCode();
        }
    }
}
