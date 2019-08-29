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

        /// <summary>
        /// Property for getting and setting the x-position
        /// </summary>
        /// <value>Must be between 0 and 19 to be valid</value>
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

        /// <summary>
        /// Property for getting and setting the y-position
        /// </summary>
        /// <value>Must be between 0 and 19 to be valid</value>
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

        /// <summary>
        /// Property for getting and setting the x-position
        /// </summary>
        /// <value>Must be between 0 and 5 to be valid</value>
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

        /// <summary>
        /// Set the playerremainingmoves attribute from player to the specified number of moves 
        /// Depending on charactertype
        /// </summary>
        public void ResetRemainingMoves()
        {
            this.PlayerRemainingMoves = this.CharacterType._moveRange; 
        }

        /// <summary>
        /// Calculates the damage taken by the player and decrement its life when someone attacks the player
        /// Depending on attackpower and attackboost of the enemy and the defensepower and defenseboost of the victim
        /// CommandGameAttack call this method when an enemy attack the player
        /// </summary>
        /// <param name="attackBoost">Attackboost of the attacking enemy</param>
        /// <param name="characterType">Charactertype of the attacking enemy</param>
        public void DecrementLife(double attackBoost, CharacterType characterType)
        {
            double totalAttackPower = attackBoost + characterType._attackPower;
            double totalDefensePower = this.DefenseBoost + this._char._defensePower;

            Life -= totalAttackPower - totalDefensePower;
        }

        /// <summary>
        /// Calculates the damage taken by the player and decrement its life when the player enter a trapfield
        /// Depending on the dealingdamage of the trap and the defensepower and defenseboost of the victim
        /// Additionally the defenseboost and defenseitem will be deleted
        /// </summary>
        /// <param name="_dealingDamage">Dealingdamage if the trap</param>
        public void DecrementLife(double _dealingDamage)
        {
            Life -= _dealingDamage - (this.DefenseBoost + this._char._defensePower);

            ResetDefenseItem();
            ResetDefenseBooster();
        }

        /// <summary>
        /// Moving the player to another field 
        /// </summary>
        /// <param name="xPosition">X-Position of the targetfield</param>
        /// <param name="yPosition">Y-Position of the targetfield</param>
        public void MovePlayer(int xPosition, int yPosition)
        {
            XPosition = xPosition;
            YPosition = yPosition;
        }

        /// <summary>
        /// Collect item from playingfield
        /// Collect a extralife when life is not maximal and add it to the player
        /// Collect a defenseboost or attackboost only when none exists yet or has a higher level and add it to the player
        /// CommandGameMove delete this item from the playingfield when this method returns true
        /// </summary>
        /// <param name="item">Item to be collected</param>
        /// <returns>Returns true when the item is collected and false when the item is ignored</returns>
        public abstract Boolean CollectItem(Item item); 

        /// <summary>
        /// Delete the attackitem
        /// ItemManagement call this method if the duration of the item is 0
        /// </summary>
        public void ResetAttackItem()
        {
            this.AttackItem = null;
        }

        /// <summary>
        /// Delete the defenseitem
        /// ItemManagement call this method if the duration of the item is 0
        /// </summary>
        public void ResetDefenseItem()
        {
            this.DefenseItem = null;
        }

        /// <summary>
        /// Set the attackboost to 0
        /// ItemManagement call this method if the duration of the attackitem is 0
        /// </summary>
        public void ResetAttackBooster()
        {
            this.AttackBoost = 0; 
        }

        /// <summary>
        /// Set the defenseboost to 0
        /// ItemManagement call this method if the duration of the defenseitem is 0
        /// </summary>
        public void ResetDefenseBooster()
        {
            this.DefenseBoost = 0; 
        }
    }
}

