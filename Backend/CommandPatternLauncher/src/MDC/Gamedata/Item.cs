using System;
namespace MDC.Gamedata
{
    public abstract class Item : FieldType
    {
        protected int _level;
        protected int _duration; 

        public Boolean CanBeAccessed(){
            return true;
        }
        public void Effects(Player player){

        }

        
    }
}