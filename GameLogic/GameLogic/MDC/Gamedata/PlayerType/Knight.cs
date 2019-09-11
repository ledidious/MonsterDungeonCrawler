using System;

namespace GameLogic.MDC.Gamedata.PlayerType
{
    [Serializable]
    public class Knight : CharacterType
    {
        public override double _attackPower => 1;
        public override double _defensePower => 0.25;
        public override int _attackRange => 1;
        public override int _moveRange => MOVE_MELEE;
    }
}
