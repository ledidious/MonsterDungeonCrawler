using System;
using MDC.Gamedata.PlayerType;

namespace MDC.Gamedata.LevelContent
{
    public class SpikeField : Trap
    {   
        protected override double _dealingDamage => 0.5;

        /// <summary>
        /// Constructor set ishidden on true
        /// </summary>
        public SpikeField()
        {
            this._isHidden = true;
        }

        /// <summary>
        /// Call decrementlife method from the player class
        /// CommandGameMove call this method if the targetfield is a trapdoor
        /// Set ishidden for the trap on false so that the trap is visible
        /// </summary>
        /// <param name="player">Player who is on the trapfield and will lost life</param>
        public override void Effects(Player player)
        {
            player.DecrementLife(this._dealingDamage);
            this._isHidden = false; 
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