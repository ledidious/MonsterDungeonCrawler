using System;
using MDC.Gamedata.PlayerType;

namespace MDC.Gamedata.LevelContent
{
    public class SpikeField : Trap
    {
        public override double _dealingDamage => 0.5;

        public override void Effects(Player player)
        {
            player.DecrementLife(this._dealingDamage);
        }

        public override void OnNextRound()
        {

        }
    }
}