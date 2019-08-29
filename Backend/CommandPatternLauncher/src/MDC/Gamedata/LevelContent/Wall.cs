using System;
using MDC.Gamedata.PlayerType;

namespace MDC.Gamedata.LevelContent
{
    public class Wall : FieldType
    {
        /// <summary>
        /// If the field can be accessed by a player
        /// The commandgamemove class call this method to prevent moves on a wall
        /// </summary>
        /// <returns>Returns false because a wall is not accesable for a player</returns>
        public Boolean CanBeAccessed(){
            return false;
        }
        
        /// <summary>
        /// Effect if the player enter the field
        /// </summary>
        /// <param name="player">Player who will be affected by the effect<</param>
        public void Effects(Player player){
            //Has no effect
        }

        /// <summary>
        /// Action on next round
        /// </summary>
        public void OnNextRound()
        {
            //Has no action on next round
        } 
    }
}