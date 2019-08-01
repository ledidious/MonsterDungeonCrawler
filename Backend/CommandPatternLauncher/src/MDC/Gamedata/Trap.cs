using System;
namespace MDC.Gamedata
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