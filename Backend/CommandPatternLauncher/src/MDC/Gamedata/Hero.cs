using System;

namespace MDC.Gamedata.PlayerType
{
    class Hero : Player
    {
        public override void CollectItem(Item item)
        {
            this._items[this._items.Length] = item;
        }
    }
}