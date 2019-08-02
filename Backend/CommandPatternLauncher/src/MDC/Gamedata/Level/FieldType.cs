using System;
using MDC.Gamedata.PlayerType;

namespace MDC.Gamedata.Level
{
    interface FieldType
    {
        Boolean CanBeAccessed(); 
        void Effects(Player player); 
        
    }
}