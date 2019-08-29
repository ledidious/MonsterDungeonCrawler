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

        public Level Level { get; set; }
    }

    [Serializable]
    public class CommandGameMove : CommandGame 
    {                                    

        public int _xPosition { get; set; }
        public int _yPosition { get; set; }

        public CommandGameMove(string sourceClientID, int xPosition, int yPosition) : base(sourceClientID)
        {
            _xPosition = xPosition;
            _yPosition = yPosition;
            IsCompleted = false;
        }

        public Boolean TargetFieldIsInvalid()
        {
            Boolean fieldIsNotAccessable = false;

            if (Level.playingField[_xPosition, _yPosition].FieldType is Wall || _xPosition == SourcePlayer.XPosition && _yPosition == SourcePlayer.YPosition)
            {
                fieldIsNotAccessable = true;
            }
            else
            {
                for (int i = 0; i < Level.playerList.Count; i++)
                {
                    if (Level.playerList[i].XPosition == _xPosition && Level.playerList[i].YPosition == _yPosition)
                    {
                        fieldIsNotAccessable = true;
                    }
                    else
                    {
                        //field is accessable
                    }
                }
            }
            return fieldIsNotAccessable;
        }

        public void TargetFieldIsTrap()
        {
            if (Level.playingField[_xPosition, _yPosition].FieldType is Trap)
            {
                Level.playingField[_xPosition, _yPosition].FieldType.Effects(SourcePlayer);
                if(Level.playingField[_xPosition, _yPosition].FieldType is Trapdoor)
                {
                    Boolean successfulMoving = false;
                    int randomX; 
                    int randomY;

                    while (successfulMoving == false)
                    {
                        randomX = new Random().Next(0, 19); 
                        randomY = new Random().Next(0, 19); 

                        if (Level.playingField[randomX, randomY].FieldType is Floor && Level.FieldBlockedByPlayer(randomX, randomY) == false)
                        {
                            successfulMoving = true;
                            SourcePlayer.MovePlayer(randomX, randomY);
                        }
                        else
                        {
                            //player can not move to a wall or another trap
                        }
                    }
                }
            }
            else
            {
                //field is not a trap
            }
        }

        public void TargetFieldContainsItem()
        {
            if (Level.playingField[_xPosition, _yPosition].FieldType is Floor && Level.playingField[_xPosition, _yPosition].Item != null)
            {
                if (SourcePlayer.CollectItem(Level.playingField[_xPosition, _yPosition].Item))
                {
                    Level.playingField[_xPosition, _yPosition].Item = null; 
                }
                else
                {
                    //player has already a higher level boost
                }
            }
            else
            {
                //field is not a item
            }
        }

        public Boolean VerifyMoveRange()
        {
            Boolean MoveInRange = false;

            //verify if targetfield is vertical in range
            if (SourcePlayer.XPosition == _xPosition)
            {
                if (_yPosition == SourcePlayer.YPosition + 1 || _yPosition == SourcePlayer.YPosition - 1)
                {
                    MoveInRange = true;
                }
                else
                {
                    //targetfield out of range
                }
            }
            else
            {
                //targetfield vertical out of range
            }

            //verify if targetfield is horizontal in range
            if (SourcePlayer.YPosition == _yPosition)
            {
                if (_xPosition == SourcePlayer.XPosition + 1 || _xPosition == SourcePlayer.XPosition - 1)
                {
                    MoveInRange = true;
                }
                else
                {
                    //targetfield out of range
                }
            }
            else
            {
                //enemy horizontal out of range
            }
            return MoveInRange;
        }



        /// <summary>
        /// set IsComplete on true if the TargetPlayer has more or equal remaining moves than the Input (_moveAmount)
        /// </summary>
        public override void Execute()
        {

            if (SourcePlayer.PlayerRemainingMoves < 1)
            {
                throw new Exceptions.CantMoveException("No more moves left"); 
            }
            else if (TargetFieldIsInvalid() == true)
            {
                throw new Exceptions.CantMoveException("Targetfield is blocked by another player or is a wall"); 
            }
            else if (VerifyMoveRange() == false)
            {
                throw new Exceptions.CantMoveException("Targetfield is out of range");
            }
            else
            {
                TargetFieldIsTrap();
                TargetFieldContainsItem(); 
                SourcePlayer.MovePlayer(_xPosition, _yPosition);
                SourcePlayer.PlayerRemainingMoves--;
                IsCompleted = true;
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
            IsCompleted = false;

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
            Boolean ObstacleInRange = false;

            //verify if an obstacle is vertical between the players
            if (SourcePlayer.XPosition == TargetPlayer.XPosition)
            {
                if (TargetPlayer.YPosition > SourcePlayer.YPosition)
                {
                    for (int i = TargetPlayer.YPosition - 1; i > SourcePlayer.YPosition; i--)
                    {
                        if (Level.playingField[SourcePlayer.XPosition, i].FieldType.CanBeAccessed() == false)
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
                        if (Level.playingField[SourcePlayer.XPosition, i].FieldType.CanBeAccessed() == false)
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
                if (TargetPlayer.XPosition > SourcePlayer.XPosition)
                {
                    for (int i = TargetPlayer.XPosition - 1; i > SourcePlayer.XPosition; i--)
                    {
                        if (Level.playingField[i, SourcePlayer.YPosition].FieldType.CanBeAccessed() == false)
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
                        if (Level.playingField[i, SourcePlayer.YPosition].FieldType.CanBeAccessed() == false)
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

            if (SourcePlayer is Monster && TargetPlayer is Monster)
            {
                throw new Exceptions.CantAttackException("Do not attack your teammember");
            }
            else if (VerifyAttackRange() == false )
            {
                throw new Exceptions.CantAttackException("Enemy is not in attack range");
            }
            else if (VerifyObstacleInRange() == true)
            {
                throw new Exceptions.CantAttackException("An obstacle is in attack range");
            }
            else if (SourcePlayer.PlayerRemainingMoves < 1)
            {
                throw new Exceptions.CantAttackException("No more moves left");
            }
            else
            {
                double attackBoost = SourcePlayer.AttackBoost;
                CharacterType characterType = SourcePlayer.CharacterType;

                TargetPlayer.DecrementLife(attackBoost, characterType);

                SourcePlayer.PlayerRemainingMoves = 0;

                IsCompleted = true;
            }
        }
    }

    [Serializable]
    public class CommandGameEndTurn : CommandGame
    {
        public CommandGameEndTurn(string SourceClientID) : base(SourceClientID)
        {
            IsCompleted = false;
        }

        public override void Execute()
        {
            SourcePlayer.PlayerRemainingMoves = 0; 
            
            IsCompleted = true; 
        }
    }
}
