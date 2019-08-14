using System;
using MDC.Server;

namespace MDC.Gamedata.LevelContent
{
    public class LaserBeam : Trap
    {

        public override double _dealingDamage => 1; 

        public Boolean IsActive; 
        
        private int _interval = 3;

        //TODO: Game class has to activate this attribute if roundcounter % 3 == 0
        public Boolean ActivateLaser
        {
            get { return IsActive; }

            set { IsActive = value; }
        }
        
         
  
    }
}