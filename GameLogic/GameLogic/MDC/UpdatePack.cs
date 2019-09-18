using System;
using System.Collections.Generic;
using GameLogic.MDC.Gamedata.PlayerType;
using GameLogic.MDC.Gamedata.LevelContent;

namespace GameLogic.MDC
{
    [Serializable]
    public class UpdatePack
    {
        //public List<Player> PlayerList { get; }
        //public List<string> ListOfClient_IDs { get; }
        public Dictionary<string, Player> PlayerOfTheGame { get; }
        public Field[,] PlayingField { get; }
        public List<Field> TrapList { get; }
        public string activeScene { get; }
        public int hash { get; }

        public UpdatePack(Dictionary<string, Player> playerOfTheGame, Field[,] playingField, List<Field> trapList, string activeScene)
        {
            //this.PlayerList = playerList;
            this.PlayerOfTheGame = playerOfTheGame;
            this.PlayingField = playingField;
            this.TrapList = trapList;
            this.activeScene = activeScene;
            //this.ListOfClient_IDs = listOfClient_IDs;
            this.hash = this.GetHashCode();
        }
    }
}
