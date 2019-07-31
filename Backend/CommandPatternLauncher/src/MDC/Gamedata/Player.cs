//Receiver
using System;

namespace MDC.Gamedata
{
    [Serializable]
    public class Player
    {
        private int _playerRemainingMoves;
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

        public string PlayerName { get; set; }

        public Player(string playerName, int moveLimit)
        {
            this.PlayerRemainingMoves = moveLimit;
            this.PlayerName = playerName;
        }

    }
}
