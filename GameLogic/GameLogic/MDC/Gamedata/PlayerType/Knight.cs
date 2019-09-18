using System;

namespace GameLogic.MDC.Gamedata.PlayerType
{
    [Serializable]
    public class Knight : CharacterType
    {
        public override double AttackPower => 1;
        public override double DefensePower => 0.25;
        public override int AttackRange => 1;
        public override int MoveRange => MOVE_MELEE;
    }
}
