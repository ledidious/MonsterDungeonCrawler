using System;
using MDC.Gamedata.LevelContent;

namespace MDC.Gamedata.PlayerType
{
    public class Hero : Player
    {

        private Item _defenseItem; 
        private Item _attackItem;

        public Hero(string playerName, CharacterType characterType, int xPosition, int yPosition)
        {
            this.PlayerName = playerName;
            this.CharacterType = characterType;
            this.XPosition = xPosition;
            this.YPosition = yPosition;

            Level.AddPlayerToLevel(this);
        }

        public void CollectItem(Item item)
        {
            if (item is ExtraLife)
            {
                this.Life ++;
            }
            else if (item is DefenseBoost)
            {
                _defenseItem = item; 
                this.DefenseBoost = _defenseItem.EffectValue;
            }
            else if (item is AttackBoost)
            {
                _attackItem = item;
                this.AttackBoost = _attackItem.EffectValue; 
            }

        }

        public void ResetBoost()
        {
            this.AttackBoost = 0; 
            this.DefenseBoost = 0; 
        }
    }
}