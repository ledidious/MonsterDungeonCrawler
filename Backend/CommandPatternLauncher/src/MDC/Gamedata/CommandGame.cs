//Command
using System;
using MDC.Gamedata.PlayerType;
using MDC.Gamedata.LevelContent;

namespace MDC.Gamedata
{
    [Serializable]
    public abstract class CommandGame : Command
    {
        public CommandGame(string SourceClientID) : base(SourceClientID)
        {
        }

        public Player SourcePlayer { get; set; }
    }

    [Serializable]
    public class CommandGameMove : CommandGame //TODO: check if target fieldtype is a trap -> activate Effects method
    {                                      //TODO: check if Player XYPosition is equal then trap XYPosition
                                           //TODO: check if target fieldtype is a floor an then check if the floor contains a item, then activate Effects method, kill the object and create floor object
                                           
        // private readonly string _direction;
        private readonly int _moveAmount;

        public CommandGameMove(string sourceClientID, int moveAmount) : base(sourceClientID)
        {
            _moveAmount = moveAmount;
            IsCompleted = false;
        }

        /// <summary>
        /// set IsComplete on true if the TargetPlayer has more or equal remaining moves than the Input (_moveAmount)
        /// </summary>
        public override void Execute()
        {
            try
            {
                if (SourcePlayer.PlayerRemainingMoves >= _moveAmount)
                {
                    SourcePlayer.PlayerRemainingMoves -= _moveAmount;

                    IsCompleted = true;
                }
            }
            catch (System.NullReferenceException e)
            {
                NullReferenceException ex = new NullReferenceException("The target object of the command is null", e);
                throw ex;
                // Console.WriteLine($"\n{e.GetType().FullName}\n{e.Message}\n{e.StackTrace}");
                // throw e;
            }


        }

    }

    [Serializable]
    public class CommandGameAttack : CommandGame
    {
        public string TargetClientID { get; set; }
        public Player TargetPlayer { get; set; }

        public CommandGameAttack(string SourceClientID, string targetClientID) : base(SourceClientID)
        {
            TargetClientID = targetClientID;

            if (SourceClientID == TargetClientID)
                throw new Exceptions.CantAttackException();
        }

        public Boolean VerifyAttackRange()
        {
            Boolean TargetInRange = false;

            //verify if an enemy is vertical in range
            if (SourcePlayer.XPosition == TargetPlayer.XPosition)
            {
                for (int i = 0; i <= SourcePlayer.CharacterType._attackRange * 2; i++)
                {
                    if (SourcePlayer.YPosition - SourcePlayer.CharacterType._attackRange + i == TargetPlayer.YPosition)
                    {
                        TargetInRange = true;
                        break;
                    }
                    else
                    {
                        //enemy out of range
                    }
                }
            }
            else
            {
                //enemy vertical out of range
            }
            
            //verify if an enemy is horizontal in range
            if (SourcePlayer.YPosition == TargetPlayer.YPosition)
            {
                for (int i = 0; i <= SourcePlayer.CharacterType._attackRange * 2; i++)
                {
                    if (SourcePlayer.XPosition - SourcePlayer.CharacterType._attackRange + i == TargetPlayer.XPosition)
                    {
                        TargetInRange = true;
                        break;
                    }
                    else
                    {
                        //enemy out of range
                    }
                }
            }
            else
            {
                //enemy horizontal out of range
            }
            return TargetInRange;
        }

        public Boolean VerifyObstacleInRange()
        {
            Boolean ObstacleInRange = true;

            //verify if an obstacle is vertical between the players
            if (SourcePlayer.XPosition == TargetPlayer.XPosition)
            {
                if (TargetPlayer.YPosition > SourcePlayer.YPosition)
                {
                    for (int i = TargetPlayer.YPosition - 1; i > SourcePlayer.YPosition; i--)
                    {
                        if (Level.playingField[SourcePlayer.XPosition, i]._fieldType.CanBeAccessed() == false)
                        {
                            ObstacleInRange = true;  
                            break;    
                        }
                    }
                }
                else
                {
                    for (int i = TargetPlayer.YPosition + 1; i < SourcePlayer.YPosition; i++)
                    {
                        if (Level.playingField[SourcePlayer.XPosition, i]._fieldType.CanBeAccessed() == false)
                        {
                            ObstacleInRange = true;  
                            break;    
                        }
                    }
                }
            }
            else
            {
                //no obstacle vertical between the players
            }

            //verify if an obstacle is horizontal between the players
            if (SourcePlayer.YPosition == TargetPlayer.YPosition)
            {
                if (TargetPlayer.XPosition > SourcePlayer.YPosition)
                {
                    for (int i = TargetPlayer.XPosition - 1; i > SourcePlayer.XPosition; i--)
                    {
                        if (Level.playingField[i, SourcePlayer.YPosition]._fieldType.CanBeAccessed() == false)
                        {
                            ObstacleInRange = true;
                            break;
                        }
                    }
                }
                else
                {
                    for (int i = TargetPlayer.XPosition + 1; i < SourcePlayer.XPosition; i++)
                    {
                        if (Level.playingField[i, SourcePlayer.YPosition]._fieldType.CanBeAccessed() == false)
                        {
                            ObstacleInRange = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                //no obstacle horizontal between the players
            }

            return ObstacleInRange;
        }



        //TODO: Mapping ID -> SourceClients for Server :: First identify type of command than execute mapping
        public override void Execute()
        {
            //TODO: Throw Exception if AttackedPlayer not reachable 

            if (SourcePlayer is Monster || TargetPlayer is Monster || VerifyAttackRange() == true || VerifyObstacleInRange() == true)
                throw new Exceptions.CantAttackException();

            double attackBoost = SourcePlayer.AttackBoost;
            CharacterType characterType = SourcePlayer.CharacterType;

            TargetPlayer.DecrementLife(attackBoost, characterType);

            SourcePlayer._playerRemainingMoves = 0;

        }

    }
}
