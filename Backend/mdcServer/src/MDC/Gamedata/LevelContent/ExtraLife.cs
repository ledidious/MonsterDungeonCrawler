using System;

namespace MDC.Gamedata.LevelContent
{
    [Serializable]
    public class ExtraLife : Item
    {
        protected const double EFFECTVALUE_EXTRALIFE = 1;

        /// <summary>
        /// contructor set effectvalue and isvisible
        /// </summary>
        public ExtraLife()
        {
            EffectValue = EFFECTVALUE_EXTRALIFE;
            IsVisible = true; 
        }

        /// <summary>
        /// This method does not concern this class, because the duration is infinite
        /// </summary>
        /// <returns>True because the duration of extralife is infinite</returns>
        public override Boolean DecrementBoosterDuration()
        {
            return true; 
        }
    }
}