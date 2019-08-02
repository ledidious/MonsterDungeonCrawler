using System;
using MDC.Gamedata.PlayerType;

namespace MDC.Gamedata
{
    public class Wall : FieldType
    {
        public Boolean CanBeAccessed(){
            return true;
        }
        public void Effects(Player player){

        }
    }
}