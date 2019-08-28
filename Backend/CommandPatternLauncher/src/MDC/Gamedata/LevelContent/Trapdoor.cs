using System;
using MDC.Gamedata.PlayerType; 

namespace MDC.Gamedata.LevelContent
{
    public class Trapdoor : Trap
    {
        protected override double _dealingDamage => 0.25; 

        public override void Effects(Player player)
        {
                player.DecrementLife(this._dealingDamage);            
        }

        public override void OnNextRound()
        {
            //has no effect
        }
    }
}