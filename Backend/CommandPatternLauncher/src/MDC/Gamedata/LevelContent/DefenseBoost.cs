using System;

namespace MDC.Gamedata.LevelContent
{
    public class DefenseBoost : Item
    {
        private int _level;
        private int _duration; 
        protected const int DURATION_DEFENSEBOOSTER = 3;
        protected const double EFFECTVALUE_DEFENSEBOOST = 0.25;

        /// <summary>
        /// Property for getting and setting the duration
        /// </summary>
        /// <value>Must be between 0 and 5 to be valid</value>
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
                    throw new System.ArgumentException("Duration must be between 0 and 5");
                }
            }
        }

        /// <summary>
        /// Property for getting and setting the level
        /// If the level is set also the duration and effectvalue will be set
        /// </summary>
        /// <value>Must be between 1 and 3 to be valid</value>
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
                    throw new System.ArgumentException("Level must be between 1 and 3");
                }

                //Set duration because it must be a booster
                Duration = DURATION_DEFENSEBOOSTER;
                EffectValue = EFFECTVALUE_DEFENSEBOOST * _level;
            }
        }

        public DefenseBoost(int Level)
        {
            this.Level = Level;
        }

        /// <summary>
        /// Decrements the duration of the item
        /// Called after every round by the game class
        /// </summary>
        /// <returns>False if the durationtime has expired and ture if the durationtime has not expired</returns>
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