using System;
using MDC.Gamedata.PlayerType; 

namespace MDC.Gamedata.LevelContent
{
    public class Trapdoor : Trap
    {
        protected override double _dealingDamage => 0.25; 

        public override void Effects(Player player)
        {
            Boolean successfulMoving = false;
                int randomX; 
                int randomY;

                player.DecrementLife(this._dealingDamage);

                while (successfulMoving == false)
                {
                    randomX = new Random().Next(1, 20); 
                    randomY = new Random().Next(1, 20); 

                    if (Level.playingField[randomX, randomY].FieldType is Floor && Level.FieldBlockedByPlayer(randomX, randomY) == false)
                    {
                        successfulMoving = true;
                        player.MovePlayer(randomX, randomY);
                    }
                    else
                    {
                        //player can not move to a wall or another trap
                    }
                }               
        }

        public override void OnNextRound()
        {
            //has no effect
        }
    }
}