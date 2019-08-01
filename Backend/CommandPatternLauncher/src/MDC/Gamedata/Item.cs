using System;
namespace MDC.Gamedata
{
    public abstract class Item : FieldType
    {
        protected int _level;
        protected int _duration; 

<<<<<<< HEAD
        public Boolean CanBeAccessed()
        {
            return true;
        }
        
        public void Effects(Player player)
        {

        }     
=======
        public Boolean CanBeAccessed(){
            return true;
        }
        public void Effects(Player player){

        }

        
>>>>>>> branch 'master' of git@github.com:ledidious/MonsterDungeonCrawler.git
    }
}