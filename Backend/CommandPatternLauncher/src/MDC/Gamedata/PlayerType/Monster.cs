using System;
using MDC.Gamedata.LevelContent;

namespace MDC.Gamedata.PlayerType
{
    public class Monster : Player
    {

        public Monster(string playerName, CharacterType characterType, int xPosition, int yPosition)
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