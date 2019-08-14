using System;
using MDC.Gamedata.PlayerType;

namespace MDC.Gamedata.LevelContent
{
    public abstract class Item 
    {
        protected const int DURATION_BOOSTER = 5;
        protected const int DURATION_EXTRALIFE = 3;
        protected int _level { get; set; }
        public int _startRound { get; set; }
        public int _duration { get; set; }

        public abstract double _effectValue { get; }

        public bool CanBeAccessed()
        {
            return true;
        }

        public int Level 
        {
            get { return _level; }

            set
            {
                if (value >= 1 && value <=3)
                {
                    _level = value;
                }
                else
                {
                    throw new System.IndexOutOfRangeException();
                }
            }
        }

        public int Duration
        {
            get { return _duration; }

            set
            {
                if (this is DefenseBoost || this is AttackBoost)
                {
                    _duration = DURATION_BOOSTER;
                }
                else if (this is ExtraLife)
                {
                    _duration = DURATION_EXTRALIFE;
                }
            }

        }

        public void Effects(Player player)
        {
     
        }

    }

}
