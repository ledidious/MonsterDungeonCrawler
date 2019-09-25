using System;
namespace Highscore
{
    public class GameResult
    {
        private string _playerName;
        private int _score;

        public GameResult(string playername, int score)
        {
            PlayerName = playername;
            _score = score; 
        }

        public GameResult()
        {
        }

        public int Score
        {
            get { return _score; }

            set => _score = value; 
        }

        public string PlayerName
        {
            get { return _playerName; }

            set => _playerName = value;
        }

        public override string ToString()
        {
            return PlayerName + " " + _score; 
        }
    }
}
