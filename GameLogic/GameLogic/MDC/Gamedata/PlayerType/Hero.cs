using System;
using GameLogic.MDC.Gamedata.LevelContent;

namespace GameLogic.MDC.Gamedata.PlayerType
{
    [Serializable]
    public class Hero : Player
    {
        protected const double LIFE_HERO = 5;

        public Hero(string playerName, CharacterType characterType, int xPosition, int yPosition)
        {
            this.PlayerName = playerName;
            this.CharacterType = characterType;
            this.XPosition = xPosition;
            this.YPosition = yPosition;
            this.Life = LIFE_HERO;
            this.PlayerRemainingMoves = characterType.MoveRange;
        }

        /// <summary>
        /// Collect item from playingfield
        /// Collect key from playingfield and set attribute haskey on true
        /// Collect a extralife when life is not maximal and add it to the player
        /// Collect a defenseboost or attackboost only when none exists yet or has a higher level and add it to the player
        /// CommandGameMove delete this item from the playingfield when this method returns true
        /// </summary>
        /// <param name="item">Item to be collected</param>
        /// <returns>Returns true when the item is collected and false when the item is ignored</returns>
        public override Boolean CollectItem(Item item)
        {
            Boolean collectSuccessfull = false;
            if (item is Key)
            {
                this.HasKey = true;
                collectSuccessfull = true;
            }
            else if (item is ExtraLife)
            {
                if (Life < LIFE_HERO)
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
            this.HasKey = true;
            return true;
        }
    }
}
