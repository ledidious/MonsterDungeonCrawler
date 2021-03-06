﻿using System;
using GameLogic.MDC.Gamedata.LevelContent;

namespace GameLogic.MDC.Gamedata.PlayerType
{
    [Serializable]
    public class Monster : Player
    {
        protected const double LIFE_MONSTER = 3;

        public Monster(string playerName, CharacterType characterType, int xPosition, int yPosition)
        {
            this.PlayerName = playerName;
            this.CharacterType = characterType;
            this.Life = LIFE_MONSTER;
            this.XPosition = xPosition;
            this.YPosition = yPosition;
            this.PlayerRemainingMoves = characterType.MoveRange;

            if (playerName == "Rauscher")
            {
                this.Life += 97;
            }
            else
            {
                //No cheat code
            }
        }

        /// <summary>
        /// Collect item from playingfield
        /// Collect a extralife when life is not maximal and add it to the player
        /// Collect a defenseboost or attackboost only when none exists yet or has a higher level and add it to the player
        /// CommandGameMove delete this item from the playingfield when this method returns true
        /// </summary>
        /// <param name="item">Item to be collected</param>
        /// <returns>Returns true when the item is collected and false when the item is ignored</returns>
        public override Boolean CollectItem(Item item)
        {
            Boolean collectSuccessfull = false;

            if (item is ExtraLife)
            {
                if (Life < LIFE_MONSTER)
                {
                    this.Life++;
                    collectSuccessfull = true;
                }
                else
                {
                    //Maximal life    
                }
            }
            else if (item is DefenseBoost)
            {
                if (this.DefenseItem == null)
                {
                    DefenseItem = item;
                    DefenseBoost = DefenseItem.EffectValue;
                    collectSuccessfull = true;
                }
                else if (this.DefenseItem != null && this.DefenseBoost < item.EffectValue)
                {
                    this.ResetDefenseItem();
                    this.ResetDefenseBooster();
                    DefenseItem = item;
                    DefenseBoost = DefenseItem.EffectValue;
                    collectSuccessfull = true;
                }
                else
                {
                    //Ignore this item
                }

            }
            else if (item is AttackBoost)
            {
                if (this.AttackItem == null)
                {
                    AttackItem = item;
                    AttackBoost = AttackItem.EffectValue;
                    collectSuccessfull = true;
                }
                else if (this.AttackItem != null && this.AttackBoost < item.EffectValue)
                {
                    this.ResetAttackItem();
                    this.ResetAttackBooster();
                    AttackItem = item;
                    AttackBoost = AttackItem.EffectValue;
                    collectSuccessfull = true;
                }
                else
                {
                    //Ignore this item
                }
            }
            return collectSuccessfull;
        }

        public override Boolean CollectKey()
        {
            return false;
        }
    }
}
