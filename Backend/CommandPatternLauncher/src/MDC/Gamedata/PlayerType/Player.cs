//Receiver
using System;
using MDC.Gamedata.LevelContent;

namespace MDC.Gamedata.PlayerType
{
    [Serializable]
    public abstract class Player
    {

        protected const double LIFE_MONSTER = 3;
        protected const double LIFE_HERO = 5;

        private int _xPosition;
        private int _yPosition;
        private int _playerRemainingMoves;
        private double _life;
        private double _attackBoost;
        private double _defenseBoost;
        private Item _attackItem;
        private Item _defenseItem;
        private CharacterType _char;
        public string PlayerName;

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

            set
            {

                _char = value;

                if (this is Monster)
                {
                    _life = LIFE_MONSTER;
                }
                else if (this is Hero)
                {
                    _life = LIFE_HERO;
                }
            }
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

        public Boolean CollectItem(Item item)
        {
            Boolean collectSuccessfull = false; 

            if (item.Equals(Level.playingField[XPosition, YPosition].Item))
            {

                if (item is ExtraLife)
                {
                    if (this is Monster && Life < LIFE_MONSTER)
                    {
                        this.Life++;
                        collectSuccessfull = true;
                    }
                    else if (this is Hero && Life < LIFE_HERO)
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
                    _defenseItem = item;
                    this.DefenseBoost = _defenseItem.EffectValue;
                    collectSuccessfull = true;
                }
                else if (item is AttackBoost)
                {
                    _attackItem = item;
                    this.AttackBoost = _attackItem.EffectValue;
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

        public void ResetBoost()
        {
            this.AttackBoost = 0;
            this.DefenseBoost = 0;
        }

    }

}

