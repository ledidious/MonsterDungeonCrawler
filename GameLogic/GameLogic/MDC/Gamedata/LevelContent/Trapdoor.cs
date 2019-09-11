using System;
using GameLogic.MDC.Gamedata.PlayerType;

namespace GameLogic.MDC.Gamedata.LevelContent
{
    [Serializable]
    public class Trapdoor : Trap
    {
        protected override double _dealingDamage => 0.25;

        /// <summary>
        /// Constructor set ishidden on true
        /// </summary>
        public Trapdoor()
        {
            this._isHidden = true;
        }

        /// <summary>
        /// Call decrementlife method from the player class 
        /// CommandGameMove call this method if the targetfield is a trapdoor
        /// Set ishidden for the trap on false so that the trap is visible
        /// </summary>
        /// <param name="player">Player who will be affected by the effect</param>
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
