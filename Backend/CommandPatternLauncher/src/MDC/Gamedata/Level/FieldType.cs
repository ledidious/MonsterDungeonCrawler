using System;
using MDC.Gamedata.PlayerType;

namespace MDC.Gamedata
{
    interface FieldType
    {
        Boolean CanBeAccessed(); 
        void Effects(Player player); 
        
    }
}