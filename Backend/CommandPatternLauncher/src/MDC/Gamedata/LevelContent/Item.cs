using System;
using MDC.Gamedata.PlayerType;

namespace MDC.Gamedata.LevelContent
{
    [Serializable]
    public abstract class Item
    {

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
