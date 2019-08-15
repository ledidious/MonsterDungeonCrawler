using System;
using MDC.Gamedata.PlayerType;

namespace MDC.Gamedata.LevelContent
{
    public abstract class Item 
    {
        protected const int DURATION_BOOSTER = 5;
        protected const int DURATION_EXTRALIFE = 3;
        protected const double EFFECTVALUE_BOOST = 0.25;
        protected const double EFFECTVALUE_EXTRALIFE = 1;


        protected int Level 
        {

            get { return Level; }

            set
            {
                if (value >= 1 && value <=3)
                {
                    Level = value;
                }
                else
                {
                    throw new System.ArgumentException();
                }

                //set duration because it must be a booster
                this.Duration = DURATION_BOOSTER; 
                this.EffectValue = EFFECTVALUE_BOOST * Level;

            }
        }

        public int Duration
        {
            get { return Duration; }

            set
            {
                if (value >= 0 && value <= 3)
                {
                    Duration = value;
                }
                else
                {
                    throw new System.ArgumentException();
                }
            }

        }

        public double EffectValue
        {
            get { return EffectValue; }

            set
            {
                if (value >= 0 && value <= 0.75)
                {
                    EffectValue = value;
                }
                else
                {
                    throw new System.ArgumentException();
                }

            }

        }

    }

}
