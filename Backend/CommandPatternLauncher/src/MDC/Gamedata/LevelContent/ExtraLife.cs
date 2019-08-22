using System;

namespace MDC.Gamedata.LevelContent
{
    public class ExtraLife : Item
    {
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