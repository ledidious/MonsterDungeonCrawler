using System;
using MDC.Gamedata.PlayerType;

namespace MDC.Gamedata.LevelContent
{
    public abstract class Item 
    {
        protected const int DURATION_BOOSTER = 3;
        protected const double EFFECTVALUE_BOOST = 0.25;
        protected const double EFFECTVALUE_EXTRALIFE = 1;

        private double _effectValue; 

        private int _level; 

        private int _duration; 


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
                    throw new System.ArgumentException();
                }

                //set duration because it must be a booster
                Duration = DURATION_BOOSTER; 
                EffectValue = EFFECTVALUE_BOOST * _level;
            }
        }

        public int Duration
        {
            get 
            {
                if (this is AttackBoost || this is DefenseBoost)
                {
                    return _duration;
                }
                else
                {
                    throw new System.ArgumentException();
                }
            }

            set
            {
                if (value >= 0 && value <= 5 && this is AttackBoost || this is DefenseBoost)
                {
                    _duration = value;
                }
                else
                {
                    throw new System.ArgumentException();
                }
            }

        }

        public double EffectValue
        {
            get { return _effectValue; }

            set
            {
                if (value >= 0 && value <= 1)
                {
                    _effectValue = value;
                }
                else
                {
                    throw new System.ArgumentException();
                }

            }

        }

    }

}
