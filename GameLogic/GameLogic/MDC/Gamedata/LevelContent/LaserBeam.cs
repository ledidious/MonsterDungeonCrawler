using System;
using GameLogic.MDC.Gamedata.PlayerType;

namespace GameLogic.MDC.Gamedata.LevelContent
{
    [Serializable]
    public class LaserBeam : Trap
    {
        protected override double _dealingDamage => 1;
        protected Boolean _isActive = false;

        /// <summary>
        /// Contructor set isHidden on false
        /// </summary>
        public LaserBeam()
        {
            this._isHidden = false;
        }

        public LaserBeam(Boolean isActive)
        {
            this._isHidden = false;
            _isActive = isActive;

        }

        public Boolean Activate
        {
            get { return _isActive; }

            set { _isActive = value; }
        }

        //TODO: Game Class has to combine with these method
        /// <summary>
        /// Set _isActive on true if it is false and false if it is true
        /// </summary>
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

        /// <summary>
        /// Call decrementlife method from the player class if the laserbeam is active
        /// CommandGameMove call this method if the targetfield is a laserbeam
        /// </summary>
        /// <param name="player">Player who is on the trapfield and will lost life</param>
        public override void Effects(Player player)
        {
            if (this.Activate == true)
            {
                player.DecrementLife(this._dealingDamage);
            }
            else
            {
                //Laser is not active 
            }
        }
    }
}
