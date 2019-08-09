using System;
using MDC.Gamedata.PlayerType;

namespace MDC.Gamedata.LevelContent
{
    public class Wall : FieldType
    {
        public Boolean CanBeAccessed(){
            return false;
        }
        public void Effects(Player player){

        }

        
    }
}