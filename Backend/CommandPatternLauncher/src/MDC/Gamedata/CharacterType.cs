using System;
namespace MDC.Gamedata
{

    public abstract class CharacterType
    {
        protected int _attackPower { get; set; }
        protected int _defensePower { get; set; }
        protected int _attackRanger { get; set; }

        public Boolean AttackEnemy(Player targetPlayer)
        {
            return true;
        }
    }
}

