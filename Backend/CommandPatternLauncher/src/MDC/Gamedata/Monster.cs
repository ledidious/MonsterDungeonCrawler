using System;

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