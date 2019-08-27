using System;
using MDC.Gamedata.LevelContent;

namespace MDC.Gamedata.PlayerType
{
    [Serializable]
    public class Monster : Player
    {
        protected const double LIFE_MONSTER = 3;

        public Monster(string playerName, CharacterType characterType, int xPosition, int yPosition)
        {
            this.PlayerName = playerName;
            this.CharacterType = characterType;
            this.XPosition = xPosition;
            this.YPosition = yPosition;
            this.Life = LIFE_MONSTER;

            Level.AddPlayerToLevel(this);
        }

        public override Boolean CollectItem(Item item)
        {
            Boolean collectSuccessfull = false;

            if (item.Equals(Level.playingField[XPosition, YPosition].Item))
            {
                if (item is ExtraLife)
                {
                    if (Life < LIFE_MONSTER)
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
                    AttackItem = item;
                    this.AttackBoost = AttackItem.EffectValue;
                    collectSuccessfull = true;
                }
                return collectSuccessfull;
            }
            else
            {
                //item out of range
                throw new System.ArgumentException();
            }
        }
    }
}