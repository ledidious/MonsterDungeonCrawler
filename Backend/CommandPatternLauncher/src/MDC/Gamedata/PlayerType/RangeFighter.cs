using System;

namespace MDC.Gamedata.PlayerType
{
    [Serializable]
    public class RangeFighter : CharacterType
    {
         private double _attackPower => 0.5;
         private double _defensePower => 0;
         private int _attackRange => 3;
         private int _moveRange => MOVE_RANGE;

    }
}
