using System;
using MDC.Gamedata.PlayerType;
using MDC.Gamedata.LevelContent;

namespace MDC.Gamedata.LevelContent
{
    public abstract class Trap : FieldType
    {
        public abstract double _dealingDamage { get; } 

        public Boolean CanBeAccessed(){
            return true;
        }

        public abstract void OnNextRound(); 

        public abstract void Effects(Player player);

      

    }
}