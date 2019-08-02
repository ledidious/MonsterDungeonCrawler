using System;
namespace MDC.Gamedata
{
    public interface FieldType
    {
        Boolean CanBeAccessed(); 
        public void Effects(Player player); 
        
    }
}