using System;
namespace GameLogic.MDC.Gamedata.PlayerType
{
    [Serializable]
    public class Archer : CharacterType
    {
        public override double AttackPower => 0.75;
        public override double DefensePower => 0;
        public override int AttackRange => 4;
        public override int MoveRange => MOVE_RANGE;
    }
}
