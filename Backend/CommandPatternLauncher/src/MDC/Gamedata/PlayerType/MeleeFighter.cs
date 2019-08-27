using System;

namespace MDC.Gamedata.PlayerType
{
    [Serializable]
    public class MeleeFighter : CharacterType
    {
         private double _attackPower => 1;
         private double _defensePower => 0.25;
         private int _attackRange => 1;
         private int _moveRange => MOVE_MELEE;
    
    }
}
