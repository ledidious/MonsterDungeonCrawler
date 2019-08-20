using System;
using MDC.Gamedata.PlayerType;
using MDC.Gamedata.LevelContent;

namespace MDC.Gamedata.LevelContent
{
    public abstract class Trap : FieldType
    {
        public abstract double _dealingDamage { get; } 

        public Boolean CanBeAccessed(){
            return true;
        }
        public void Effects(Player player)
        {
            player.DecrementLife(this._dealingDamage);

            if (this is Trapdoor)
            {
                Boolean successfulMoving = false;
                int randomX; 
                int randomY;

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
            else
            {
                //do not move the player
            }
        }
    }
}