using System;
using MDC.Gamedata.PlayerType;

namespace MDC.Gamedata.LevelContent
{
    public abstract class Trap : FieldType
    {
        public abstract double _dealingDamage { get; } 

        public Boolean CanBeAccessed(){
            return true;
        }
        public void Effects(Player player) => player.DecrementLife(this._dealingDamage);
    }
}