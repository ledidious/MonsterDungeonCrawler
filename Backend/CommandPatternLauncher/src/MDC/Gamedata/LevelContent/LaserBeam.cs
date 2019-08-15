using System;
using MDC.Server;

namespace MDC.Gamedata.LevelContent
{
    public class LaserBeam : Trap
    {

        public override double _dealingDamage => 1; 

        private Boolean _isActive; 
        
        public int _interval = 3;

        //TODO: Game class has to activate this attribute if roundcounter % 3 == 0
        public Boolean ActivateLaser
        {
            get { return _isActive; }

            set { _isActive = value; }
        }
        
         
  
    }
}