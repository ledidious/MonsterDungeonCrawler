using System;

namespace MDC.Gamedata.PlayerType
{
    [Serializable]
    public abstract class CharacterType
    {
        public abstract double _attackPower { get; }
        public abstract double _defensePower { get; }
        public abstract int _attackRange { get; }

    }    
}

