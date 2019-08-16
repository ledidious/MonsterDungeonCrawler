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

        public int _xPosition;
        public int _yPosition;
        private int _playerRemainingMoves;
        private double _life;
        private double _attackBoost;
        private double _defenseBoost;
        private CharacterType _char;
        public string PlayerName;

        public int XPosition
        {
            get { return _xPosition; }
            set 
            {
                if (value >= 0 && value <= 20)
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
                if (value >= 0 && value <= 20)
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

        public double AttackBoost
        {
            get { return _attackBoost; }
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
                if (value > 0 && value <= 20)
                {
                    this._playerRemainingMoves = value;
                }
                else
                {
                    this._playerRemainingMoves = -999;
                }
            }
        }

        public void DecrementLife(double attackBoost, CharacterType characterType)
        {
            double totalAttackPower = attackBoost + characterType._attackPower;
            double totalDefensePower = this._defenseBoost + this._char._defensePower;

            _life -= totalAttackPower - totalDefensePower;

            this._defenseBoost = 0; 
        }

        public void DecrementLife(double _dealingDamage)
        {
           _life -= _dealingDamage - (this._defenseBoost + this._char._defensePower);

            this._defenseBoost = 0; 
        }

        public void MovePlayer(int xPosition, int yPosition)
        {
            XPosition = xPosition;
            YPosition = yPosition;
        }

    }

}

