using System;
using MDC.Gamedata.LevelContent;

namespace MDC.Gamedata.PlayerType
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
            this.PlayerRemainingMoves = characterType._moveRange; 

            Level.AddPlayerToLevel(this);
        }

        public override Boolean CollectItem(Item item)
        {
            Boolean collectSuccessfull = false;

            if (item is ExtraLife)
            {
                if (Life < LIFE_HERO)
                {
                    this.Life++;
                    collectSuccessfull = true;
                }
                else
                {
                    //maximal life    
                }
            }
            else if (item is DefenseBoost)
            {
                if (this.DefenseItem != null)
                   {
                       this.ResetDefenseItem();
                       this.ResetDefenseBooster(); 
                   }
                DefenseItem = item;
                DefenseBoost = DefenseItem.EffectValue;
                collectSuccessfull = true;
            }
            else if (item is AttackBoost)
            {
                if (this.AttackItem != null)
                   {
                       this.ResetAttackItem();
                       this.ResetAttackBooster(); 
                }
                AttackItem = item;
                this.AttackBoost = AttackItem.EffectValue;
                collectSuccessfull = true;
            }
            return collectSuccessfull;
        }
    }
}