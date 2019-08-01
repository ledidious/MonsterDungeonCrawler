//Receiver
using System;

namespace MDC.Gamedata
{
    [Serializable]
    public abstract class Player
    {
        private int _playerRemainingMoves { get; set; }
        private int _life { get; set; }
        private int _attackBoost { get; set; }
        private int _defenseBoost { get; set; }
        private int[] _items { get; set; }
        private int[][] _position { get; set; }
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

        

        /*
        public Player(string playerName, int moveLimit)
        {
            this.PlayerRemainingMoves = moveLimit;
            this.PlayerName = playerName;

        }
        */

    }
}
