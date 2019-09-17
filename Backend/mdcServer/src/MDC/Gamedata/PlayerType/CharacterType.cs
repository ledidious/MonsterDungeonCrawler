using System;

namespace MDC.Gamedata.PlayerType
{
    [Serializable]
    public abstract class CharacterType
    {
        protected const int MOVE_RANGE = 2;
        protected const int MOVE_MELEE = 5;
        
        public abstract double AttackPower { get; }
        public abstract double DefensePower  { get; }
        public abstract int AttackRange  { get; }
        public abstract int MoveRange  { get; }
    }    
}

