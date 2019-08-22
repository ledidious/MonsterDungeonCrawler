using System;

namespace MDC.Gamedata.LevelContent
{
    public class DefenseBoost : Item
    {
        private int _level;

        private int _duration; 

        public int Duration
        {
            get { return _duration; }

            set
            {
                if (value >= 0 && value <= 5)
                {
                    _duration = value;
                }
                else
                {
                    throw new System.ArgumentException();
                }
            }
        }

        public int Level
        {

            get { return _level; }

            set
            {
                if (value >= 1 && value <= 3)
                {
                    _level = value;
                }
                else
                {
                    throw new System.ArgumentException();
                }

                //set duration because it must be a booster
                Duration = DURATION_BOOSTER;
                EffectValue = EFFECTVALUE_BOOST * _level;
            }
        }

        public DefenseBoost(int Level)
        {
            this.Level = Level;
        }

        public override Boolean DecrementBoosterDuration()
        {
            Boolean BoosterActive; 
            this.Duration--; 

            if (this.Duration == 0)
            {
                BoosterActive = false;
            }
            else
            {
                BoosterActive = true; 
            }

            return BoosterActive; 
        }
    }
}