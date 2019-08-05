using System;
using MDC.Gamedata.PlayerType;

namespace MDC.Gamedata.PlayerType
{
    public class RangeFighter : CharacterType
    {
         public override double _attackPower => 0.5;
         public override double _defensePower => 0;
         public override double _attackRange => 3;

    }
}
