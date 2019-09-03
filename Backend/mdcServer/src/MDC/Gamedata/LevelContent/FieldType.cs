using System;
using MDC.Gamedata.PlayerType;

namespace MDC.Gamedata.LevelContent
{
    public interface FieldType
    {
        Boolean CanBeAccessed(); 
        void Effects(Player player); 
        void OnNextRound();  
        Boolean IsHidden();   
    }
}