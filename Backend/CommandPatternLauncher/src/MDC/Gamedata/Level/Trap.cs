using System;
using MDC.Gamedata.PlayerType;

namespace MDC.Gamedata.Level
{
    public abstract class Trap : FieldType
    {
        private int _dealingDamage;

        public Boolean CanBeAccessed(){
            return true;
        }
        public void Effects(Player player){

        }
    }
}