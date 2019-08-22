using System;
using MDC.Gamedata.PlayerType;

namespace MDC.Gamedata.LevelContent
{
    public class Floor : FieldType
    { 
        public Boolean CanBeAccessed(){
            return true;
        }
       
        public void Effects(Player player){
            //has no effect
            throw new NotImplementedException();
        }

        public void OnNextRound()
        {

        }
 
    }
}