using System;
using MDC.Gamedata.PlayerType;

namespace MDC.Gamedata.LevelContent
{
    [Serializable]
    public abstract class Item
    {
        protected const int DURATION_BOOSTER = 3;
        protected const double EFFECTVALUE_BOOST = 0.25;
        protected const double EFFECTVALUE_EXTRALIFE = 1;

        private double _effectValue;

        public abstract Boolean DecrementBoosterDuration(); 

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
