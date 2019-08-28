//Receiver
using System;
using MDC.Gamedata.LevelContent;

namespace MDC.Gamedata.PlayerType
{
    [Serializable]
    public abstract class Player
    {
        private int _xPosition;
        private int _yPosition;
        private int _playerRemainingMoves;
        private double _life;
        private double _attackBoost;
        private double _defenseBoost;
        private Item _attackItem;
        private Item _defenseItem;
        private CharacterType _char;
        private string _playerName;

        public string PlayerName
        {
            get { return _playerName; }
            set => _playerName = value;
        }

        public Item AttackItem
        {
            get { return _attackItem; }
            set => _attackItem = value;
        }
        public Item DefenseItem
        {
            get { return _defenseItem; }
            set => _defenseItem = value;
        }

        public double AttackBoost
        {
            get { return _attackBoost; }
            set => _attackBoost = value;
        }

        public double DefenseBoost
        {
            get { return _defenseBoost; }
            set => _defenseBoost = value;
        }

        public int XPosition
        {
            get { return _xPosition; }
            set
            {
                if (value >= 0 && value <= 19)
                {
                    _xPosition = value;
                }
                else
                {
                    throw new System.ArgumentException();
                }
            }
        }

        public int YPosition
        {
            get { return _yPosition; }
            set
            {
                if (value >= 0 && value <= 19)
                {
                    _yPosition = value;
                }
                else
                {
                    throw new System.ArgumentException();
                }
            }
        }

        public double Life
        {
            get { return _life; }

            set => _life = value;
        }

        public CharacterType CharacterType
        {
            get { return _char; }

            set =>_char = value;
        }

        public int PlayerRemainingMoves
        {
            get { return _playerRemainingMoves; }

            set
            {
                if (value >= 0 && value <= 5)
                {
                    this._playerRemainingMoves = value;
                }
                else
                {
                    throw new System.ArgumentException();
                }
            }
        }

        public void DecrementLife(double attackBoost, CharacterType characterType)
        {
            double totalAttackPower = attackBoost + characterType._attackPower;
            double totalDefensePower = this.DefenseBoost + this._char._defensePower;

            Life -= totalAttackPower - totalDefensePower;
        }

        public void DecrementLife(double _dealingDamage)
        {
            Life -= _dealingDamage - (this.DefenseBoost + this._char._defensePower);

            this.DefenseBoost = 0;
        }

        public void MovePlayer(int xPosition, int yPosition)
        {
            XPosition = xPosition;
            YPosition = yPosition;
        }

        public abstract Boolean CollectItem(Item item); 

        public void ResetAttackItem()
        {
            this.AttackItem = null;
        }

        public void ResetDefenseItem()
        {
            this.DefenseItem = null;
        }

        public void ResetAttackBooster()
        {
            this.AttackBoost = 0; 
        }

        public void ResetDefenseBooster()
        {
            this.DefenseBoost = 0; 
        }
    }
}

