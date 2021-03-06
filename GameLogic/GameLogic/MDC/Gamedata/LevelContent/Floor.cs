﻿using System;
using GameLogic.MDC.Gamedata.PlayerType;

namespace GameLogic.MDC.Gamedata.LevelContent
{
    [Serializable]
    public class Floor : FieldType
    {
        /// <summary>
        /// If the field can be accessed by a player
        /// The commandgamemove class call this method to prevent moves on a wall
        /// </summary>
        /// <returns>Returns true because a floor is accesable for a player</returns>
        public Boolean CanBeAccessed()
        {
            return true;
        }

        /// <summary>
        /// Effect if the player enter the field
        /// </summary>
        /// <param name="player">Player who will be affected by the effect</param>
        public void Effects(Player player)
        {
            //Has no effect 
            throw new NotImplementedException("Floor has no effect");
        }

        /// <summary>
        /// Action on next round
        /// </summary>
        public void OnNextRound()
        {
            //Has no action on next round
        }

        /// <summary>
        /// Returns if this fieldtype is hidden or not
        /// </summary>
        /// <returns>Returns false because a wall is never hidden</returns>
        public Boolean IsHidden()
        {
            return false;
        }
    }
}
