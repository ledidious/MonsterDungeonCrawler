using System;
using MDC.Gamedata.Level;

namespace MDC.Gamedata.PlayerType
{
    public class Hero : Player
    {

        public Hero(string playerName, CharacterType characterType, int xPosition, int yPosition)
        {
            this.PlayerName = playerName;
            this.CharacterType = characterType;
            this.XPosition = xPosition;
            this.YPosition = yPosition;
        }

        public override void CollectItem(Item item)
        {

        }
    }
}