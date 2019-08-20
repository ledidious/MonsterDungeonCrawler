using System;
using MDC.Gamedata.PlayerType;

namespace MDC.Gamedata.PlayerType
{
    [Serializable]
    public class RangeFighter : CharacterType
    {
         public override double _attackPower => 0.5;
         public override double _defensePower => 0;
         public override int _attackRange => 3;
         public override int _moveRange => MOVE_RANGE;

    }
}
