using System;
using MDC.Gamedata.PlayerType;

namespace MDC.Gamedata.LevelContent
{
    [Serializable]
    public abstract class Item
    {
        private double _effectValue;
        private Boolean _isVisible; 
        public abstract Boolean DecrementBoosterDuration(); 

        /// <summary>
        /// Property for getting and setting the effectvalue
        /// </summary>
        /// <value>Must be between 0 and 1 to be valid</value>
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
                    throw new System.ArgumentException("Effectvalue must be between 0 and 1");
                }
            }
        }

        /// <summary>
        /// Property for getting and setting the isvisible
        /// </summary>
        /// <value>Set only for key</value>
        public Boolean IsVisible
        {
            get { return _isVisible; }

            set => _isVisible = value; 
        }
    }
}
