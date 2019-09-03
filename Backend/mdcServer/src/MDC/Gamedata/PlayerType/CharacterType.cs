using System;

namespace MDC.Gamedata.PlayerType
{
    [Serializable]
    public abstract class CharacterType
    {
        protected const int MOVE_RANGE = 2;
        protected const int MOVE_MELEE = 5;
        
        public abstract double _attackPower { get; }
        public abstract double _defensePower  { get; }
        public abstract int _attackRange  { get; }
        public abstract int _moveRange  { get; }
    }    
}

