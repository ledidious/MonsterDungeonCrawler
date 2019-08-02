using System;
using MDC.Gamedata.Level;

namespace MDC.Gamedata.PlayerType
{
    class Monster : Player
    {
        public override void CollectItem(Item item)
        {
            this._items[this._items.Length] = item;
        }
    }
}