using System;
using MDC.Gamedata.PlayerType; 

namespace MDC.Gamedata.LevelContent
{
    public class Trapdoor : Trap
    {
        protected override double _dealingDamage => 0.25; 

        /// <summary>
        /// Call decrementlife method from the player class 
        /// CommandGameMove call this method if the targetfield is a trapdoor
        /// </summary>
        /// <param name="player">Player who will be affected by the effect</param>
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