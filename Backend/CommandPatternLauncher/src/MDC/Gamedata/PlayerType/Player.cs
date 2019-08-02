//Receiver
using System;
using MDC.Gamedata.Level;

namespace MDC.Gamedata.PlayerType
{
    [Serializable]
    public abstract class Player
    {
        private int _playerRemainingMoves { get; set; }
        protected int _life { get; set; }
        protected int _attackBoost { get; set; }
        protected int _defenseBoost { get; set; }
        protected Item[] _items { get; set; }
        protected int[][] _position { get; set; }
        private CharacterType _char { get; set; }
        public string PlayerName { get; set; }

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

        public Boolean AttackEnemy(Player targetPlayer)
        {
            return false;
        }

        

        /*
        public Player(string playerName, int moveLimit)
        {
            this.PlayerRemainingMoves = moveLimit;
            this.PlayerName = playerName;

        }
        */

    }

    // public class Hero : Player
    // {
    //     public Hero(string playerName, int moveLimit){
    //         this.PlayerRemainingMoves = moveLimit;
    //         this.PlayerName = playerName;
    //     }

    //     public override void CollectItem(Item item)
    //     {
    //         this._items[this._items.Length] = item;
    //     }
    // }

    // class Monster : Player
    // {
    //     public override void CollectItem(Item item)
    //     {
    //         this._items[this._items.Length] = item;
    //     }
    // }
}

