using System;

namespace MDC.Gamedata.LevelContent
{
    public class ExtraLife : Item
    {
        protected const double EFFECTVALUE_EXTRALIFE = 1;
        public ExtraLife()
        {
            EffectValue = EFFECTVALUE_EXTRALIFE;
        }

        public override Boolean DecrementBoosterDuration()
        {
            return true; 
        }
    }
}