using System;
using MDC.Gamedata.Level;

namespace MDC.Gamedata.PlayerType
{
    public class Hero : Player
    {   
        
        public Hero(string playerName, CharacterType characterType, int[,] position){
            this.PlayerName = playerName;
            this.CharacterType = characterType;
            this._position = position; 
        }

        public override void CollectItem(Item item)
        {
            this._items[this._items.Length] = item;
        }
    }
}