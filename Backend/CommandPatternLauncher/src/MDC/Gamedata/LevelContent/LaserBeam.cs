using System;

namespace MDC.Gamedata.LevelContent
{
    public class LaserBeam : Trap
    {

        public override double _dealingDamage => 1; 

        public Boolean _isActive { get; set; } 
        
        private int _interval = 3;


        //TODO: connect with playround counter
        /*
        
        public Boolean ActivateLaser
        {
            get { return _isActive; }

            set 
            { 
                if(ROUNDCOUNTER % _interval == 0)
                {
                    _isActive = true;
                }
                else
                {
                    _isActive = false; 
                }
            
            }
        }
        
         */
  
    }
}