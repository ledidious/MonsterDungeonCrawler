using System;
namespace MDC.Gamedata
{
    public interface FieldType
    {
        Boolean CanBeAccessed(); 
        void Effects(Player player); 
        
    }
}