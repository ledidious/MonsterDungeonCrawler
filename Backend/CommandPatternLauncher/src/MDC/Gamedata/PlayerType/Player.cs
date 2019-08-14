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

        public int XPosition;
        public int YPosition;
        public int _playerRemainingMoves { get; set; }
        public double _life { get; set; }
        protected double _attackBoost { get; set; }
        protected double _defenseBoost { get; set; }
        public Item[] Items;
        protected CharacterType _char { get; set; }
        public string PlayerName { get; set; }

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

        public abstract void CollectItem(Item item);

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

    }

}

