using System;
using MDC.Gamedata.PlayerType;
using MDC.Gamedata.LevelContent;

namespace MDC.Gamedata.LevelContent
{
    public abstract class Trap : FieldType
    {
        protected abstract double _dealingDamage { get; } 

        /// <summary>
        /// If the field can be accessed by a player
        /// The commandgamemove class call this method to prevent moves on a wall
        /// </summary>
        /// <returns>Returns true because trapfields are accessable for a player</returns>
        public Boolean CanBeAccessed(){
            return true;
        }

        /// <summary>
        /// Action on next round
        /// </summary>
        public abstract void OnNextRound(); 

        /// <summary>
        /// Effect if the player enter the field
        /// </summary>
        /// <param name="player">Player who will be affected by the effect</param>
        public abstract void Effects(Player player); 
    }
}