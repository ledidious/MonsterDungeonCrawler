using System;

namespace MDC.Gamedata.PlayerType
{
    [Serializable]
    public abstract class CharacterType
    {
        protected const int MOVE_RANGE = 2;
        protected const int MOVE_MELEE = 5;
        
        private double _attackPower;
        private double _defensePower;
        private int _attackRange;
        private int _moveRange;

        public double Attackpower
        {
            get { return _attackPower; }
        }

        public double Defensepower
        {
            get { return _defensePower; }
        }

        public int Attackrange
        {
            get { return _attackRange; }
        }
        
        public int MoveRange
        {
            get { return _moveRange; }
        }
    }    
}

