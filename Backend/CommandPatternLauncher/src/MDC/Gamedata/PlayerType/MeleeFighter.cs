using System;

namespace MDC.Gamedata.PlayerType
{
    public class MeleeFighter : CharacterType
    {
         public override double _attackPower => 1;
         public override double _defensePower => 0.25;
         public override int _attackRange => 1;
    
    }
}
