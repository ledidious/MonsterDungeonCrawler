using System;
namespace MDC.Gamedata
{
    public abstract class Item : FieldType
    {
        protected int _level;
        protected int _duration;

        public bool CanBeAccessed()
        {
            throw new NotImplementedException();
        }

        public void Effects(Player player)
        {
            throw new NotImplementedException();
        }
    }
}
