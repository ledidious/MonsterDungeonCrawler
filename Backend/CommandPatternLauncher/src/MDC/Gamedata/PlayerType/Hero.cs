using System;
using MDC.Gamedata.LevelContent;

namespace MDC.Gamedata.PlayerType
{
    public class Hero : Player
    {

        private Item _item; 

        public Hero(string playerName, CharacterType characterType, int xPosition, int yPosition)
        {
            this.PlayerName = playerName;
            this.CharacterType = characterType;
            this.XPosition = xPosition;
            this.YPosition = yPosition;

            Level.AddPlayerToLevel(this);
        }

        public void CollectItem(Item item)
        {

        }
    }
}