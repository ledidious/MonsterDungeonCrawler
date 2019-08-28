using System;
using MDC.Gamedata.PlayerType; 

namespace MDC.Gamedata.LevelContent
{
    public class LaserBeam : Trap
    {
        protected override double _dealingDamage => 1; 

        protected Boolean _isActive = false; 

        public Boolean Activate
        {
            get { return _isActive; }

            set { _isActive = value; }
        } 

        //TODO: Game Class has to combine with these method
        public override void OnNextRound()
        {
            if (_isActive == false)
            {
                Activate = true; 
            }
            else
            {
                Activate = false; 
            }           
        }    
        
        public override void Effects(Player player)
        {
            if (this.Activate == true)
            {
                player.DecrementLife(this._dealingDamage);
            }
            else
            {
                //laser is not active 
            }
        }
    }
}