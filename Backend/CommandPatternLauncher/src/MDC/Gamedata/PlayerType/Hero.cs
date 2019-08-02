using System;
using MDC.Gamedata.Level;

namespace MDC.Gamedata.PlayerType
{
    public class Hero : Player
    {
        public Hero(string playerName, int moveLimit){
            this.PlayerRemainingMoves = moveLimit;
            this.PlayerName = playerName;
        }

        public override void CollectItem(Item item)
        {
            this._items[this._items.Length] = item;
        }
    }
}