using System;
namespace MDC.Gamedata
{

    public abstract class CharacterType
    {
        private int _attackPower { get; set; }
        private int _defensePower { get; set; }
        private int _attackRanger { get; set; }

        public Boolean AttackEnemy(Player targetPlayer)
        {
            return true;
        }

    }
}

