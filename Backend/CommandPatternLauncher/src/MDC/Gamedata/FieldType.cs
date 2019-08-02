using System;
namespace MDC.Gamedata
{
    interface FieldType
    {
        Boolean CanBeAccessed(); 
        void Effects(Player player); 
        
    }
}