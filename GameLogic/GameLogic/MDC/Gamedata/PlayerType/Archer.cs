using System;
namespace GameLogic.MDC.Gamedata.PlayerType
{
    [Serializable]
    public class Archer : CharacterType
    {
        public override double AttackPower => 0.5;
        public override double DefensePower => 0;
        public override int AttackRange => 3;
        public override int MoveRange => MOVE_RANGE;
    }
}
