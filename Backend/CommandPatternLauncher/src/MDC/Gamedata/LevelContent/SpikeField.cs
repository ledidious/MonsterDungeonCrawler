using System;
using MDC.Gamedata.PlayerType;

namespace MDC.Gamedata.LevelContent
{
    public class SpikeField : Trap
    {
        protected override double _dealingDamage => 0.5;

        /// <summary>
        /// Call decrementlife method from the player class
        /// </summary>
        /// <param name="player">Player who is on the trapfield and will lost life</param>
        public override void Effects(Player player)
        {
            player.DecrementLife(this._dealingDamage);
        }

        /// <summary>
        /// Action on next round
        /// </summary>
        public override void OnNextRound()
        {
            //Has no action on next round
        }
    }
}