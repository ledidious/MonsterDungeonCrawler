﻿using System;
using GameLogic.MDC.Gamedata.PlayerType;
using GameLogic.MDC.Gamedata.LevelContent;

namespace GameLogic.MDC.Gamedata
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

    /// <summary>
    /// Command game move.
    /// </summary>
    [Serializable]
    public class CommandGameMove : CommandGame
    {

        public int XPosition { get; set; }
        public int YPosition { get; set; }

        /// <summary>
        /// Command for moving the player on a selected field
        /// Set iscompleted on false
        /// </summary>
        /// <param name="sourceClientID">ID of the Client who wants to move his player</param>
        /// <param name="xPosition">Target x-position</param>
        /// <param name="yPosition">Target y-position</param>
        public CommandGameMove(string sourceClientID, int xPosition, int yPosition) : base(sourceClientID)
        {
            XPosition = xPosition;
            YPosition = yPosition;
            IsCompleted = false;
        }

        /// <summary>
        /// Checks if the targetfield is accessable for the player or not
        /// </summary>
        /// <returns>False if the targetfield is a wall, blocked by another player or the current position of the player and true if these conditions are not given</returns>
        public Boolean TargetFieldIsInvalid()
        {
            Boolean fieldIsNotAccessable = false;

            if (Level.PlayingField[XPosition, YPosition].FieldType is Wall || XPosition == SourcePlayer.XPosition && YPosition == SourcePlayer.YPosition)
            {
                fieldIsNotAccessable = true;
            }
            else
            {
                for (int i = 0; i < Level.PlayerList.Count; i++)
                {
                    if (Level.PlayerList[i].XPosition == XPosition && Level.PlayerList[i].YPosition == YPosition)
                    {
                        fieldIsNotAccessable = true;
                    }
                    else
                    {
                        //Field is accessable
                    }
                }
            }
            return fieldIsNotAccessable;
        }

        /// <summary>
        /// Checks if the targetfield is a trap
        /// If the target field is a trap it will be triggered and if it is a trapdoor the player will be additionally set to a random free and accessable field
        /// </summary>
        public void TargetFieldIsTrap()
        {
            if (Level.PlayingField[XPosition, YPosition].FieldType is Trap)
            {
                Level.PlayingField[XPosition, YPosition].FieldType.Effects(SourcePlayer);
            }
            else
            {
                //Field is not a trap
            }
        }

        /// <summary>
        /// Checks if the targetfield contains a item
        /// If the targetfield contains a visible item it will be collected and deleted from the playingfield
        /// </summary>
        public void TargetFieldContainsItem()
        {
            if (Level.PlayingField[XPosition, YPosition].FieldType is Floor && Level.PlayingField[XPosition, YPosition].Item != null && Level.PlayingField[XPosition, YPosition].Item.IsVisible == true)
            {
                if (SourcePlayer.CollectItem(Level.PlayingField[XPosition, YPosition].Item) == true)
                {
                    Level.PlayingField[XPosition, YPosition].Item = null;
                }
                else
                {
                    //Player has already a higher level boost
                }

            }
            else
            {
                //Field is not a item
            }
        }

        /// <summary>
        /// Checks if the targetfield is in moverange
        /// </summary>
        /// <returns>True when the move is in range and false when the move is out of range</returns>
        public Boolean VerifyMoveRange()
        {
            Boolean MoveInRange = false;

            //Verify if targetfield is vertical in range
            if (SourcePlayer.XPosition == XPosition)
            {
                if (YPosition == SourcePlayer.YPosition + 1 || YPosition == SourcePlayer.YPosition - 1)
                {
                    MoveInRange = true;
                }
                else
                {
                    //Targetfield out of range
                }
            }
            else
            {
                //Targetfield vertical out of range
            }

            //Verify if targetfield is horizontal in range
            if (SourcePlayer.YPosition == YPosition)
            {
                if (XPosition == SourcePlayer.XPosition + 1 || XPosition == SourcePlayer.XPosition - 1)
                {
                    MoveInRange = true;
                }
                else
                {
                    //Targetfield out of range
                }
            }
            else
            {
                //Enemy horizontal out of range
            }
            return MoveInRange;
        }



        /// <summary>
        /// Set iscompleted on true, call targetfieldistrap method, call targetfieldcontainsitem method, move the player
        /// and decrement the playerremainingmoves when the sourceplayer has more moves left, the targetfield is valid and the targetfield is in range
        /// </summary>
        public override void Execute()
        {

            if (SourcePlayer.PlayerRemainingMoves < 1)
            {
                throw new CantMoveException("No more moves left");
            }
            else if (TargetFieldIsInvalid() == true)
            {
                throw new CantMoveException("Targetfield is blocked by another player or is a wall");
            }
            else if (VerifyMoveRange() == false)
            {
                throw new CantMoveException("Targetfield is out of range");
            }
            else
            {
                TargetFieldIsTrap();
                TargetFieldContainsItem();
                if (!(Level.PlayingField[XPosition, YPosition].FieldType is Trapdoor))
                {
                    SourcePlayer.MovePlayer(XPosition, YPosition);
                }
                SourcePlayer.PlayerRemainingMoves--;
                IsCompleted = true;
            }
        }
    }

    /// <summary>
    /// Command game attack.
    /// </summary>
    [Serializable]
    public class CommandGameAttack : CommandGame
    {
        public string TargetClientID { get; set; }
        public Player TargetPlayer { get; set; }

        /// <summary>
        /// Command for attack an enemy
        /// Set iscompleted on false
        /// </summary>
        /// <param name="SourceClientID">ID of the Client who wants to attack</param>
        /// <param name="targetClientID">ID of the Client which will be attecked</param>
        /// <returns></returns>
        public CommandGameAttack(string SourceClientID, string targetClientID) : base(SourceClientID)
        {
            IsCompleted = false;

            TargetClientID = targetClientID;

            if (SourceClientID == TargetClientID)
                throw new CantAttackException();
        }

        /// <summary>
        /// Checks if the targetplayer is in the attackrange of the sourceplayer
        /// </summary>
        /// <returns>True when the targetplayer is in the attackrange of the sourceplayer and false when the targtplayer is not in the attackrange of the sourceplayer</returns>
        public Boolean VerifyAttackRange()
        {
            Boolean TargetInRange = false;

            //Verify if an enemy is vertical in range
            if (SourcePlayer.XPosition == TargetPlayer.XPosition)
            {
                for (int i = 0; i <= SourcePlayer.CharacterType.AttackRange * 2; i++)
                {
                    if (SourcePlayer.YPosition - SourcePlayer.CharacterType.AttackRange + i == TargetPlayer.YPosition)
                    {
                        TargetInRange = true;
                        break;
                    }
                    else
                    {
                        //Enemy out of range
                    }
                }
            }
            else
            {
                //Enemy vertical out of range
            }

            //Verify if an enemy is horizontal in range
            if (SourcePlayer.YPosition == TargetPlayer.YPosition)
            {
                for (int i = 0; i <= SourcePlayer.CharacterType.AttackRange * 2; i++)
                {
                    if (SourcePlayer.XPosition - SourcePlayer.CharacterType.AttackRange + i == TargetPlayer.XPosition)
                    {
                        TargetInRange = true;
                        break;
                    }
                    else
                    {
                        //Enemy out of range
                    }
                }
            }
            else
            {
                //Enemy horizontal out of range
            }
            return TargetInRange;
        }

        /// <summary>
        /// Checks if an obstacle is between the sourceplayer and the targetplayer
        /// </summary>
        /// <returns>True when an obstacle is between the sourceplayer and the targetplayer and false when there is not an obstacle between the sourceplayer and the targetplayer</returns>
        public Boolean VerifyObstacleInRange()
        {
            Boolean ObstacleInRange = false;

            //Verify if an obstacle is vertical between the players
            if (SourcePlayer.XPosition == TargetPlayer.XPosition)
            {
                if (TargetPlayer.YPosition > SourcePlayer.YPosition)
                {
                    for (int i = TargetPlayer.YPosition - 1; i > SourcePlayer.YPosition; i--)
                    {
                        if (Level.PlayingField[SourcePlayer.XPosition, i].FieldType.CanBeAccessed() == false)
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
                        if (Level.PlayingField[SourcePlayer.XPosition, i].FieldType.CanBeAccessed() == false)
                        {
                            ObstacleInRange = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                //No obstacle vertical between the players
            }

            //Verify if an obstacle is horizontal between the players
            if (SourcePlayer.YPosition == TargetPlayer.YPosition)
            {
                if (TargetPlayer.XPosition > SourcePlayer.XPosition)
                {
                    for (int i = TargetPlayer.XPosition - 1; i > SourcePlayer.XPosition; i--)
                    {
                        if (Level.PlayingField[i, SourcePlayer.YPosition].FieldType.CanBeAccessed() == false)
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
                        if (Level.PlayingField[i, SourcePlayer.YPosition].FieldType.CanBeAccessed() == false)
                        {
                            ObstacleInRange = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                //No obstacle horizontal between the players
            }
            return ObstacleInRange;
        }


        /// <summary>
        /// Set iscompleted on true, call decrementlife method and set playerremainingmoves to 0 when source- or targetplayer is the hero,
        /// the targetplayer is in attackrange, there is no obstacle between source- and targetplayer and the sourceplayer has more moves left
        /// </summary>
        public override void Execute()
        {
            if (SourcePlayer is Monster && TargetPlayer is Monster)
            {
                throw new CantAttackException("Do not attack your teammember");
            }
            else if (VerifyAttackRange() == false)
            {
                throw new CantAttackException("Enemy is not in attack range");
            }
            else if (VerifyObstacleInRange() == true)
            {
                throw new CantAttackException("An obstacle is in attack range");
            }
            else if (SourcePlayer.PlayerRemainingMoves < 1)
            {
                throw new CantAttackException("No more moves left");
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

    /// <summary>
    /// Command game end turn.
    /// </summary>
    [Serializable]
    public class CommandGameEndTurn : CommandGame
    {

        /// <summary>
        /// Command to finish the round with remaining moves
        /// Set iscompletet on false 
        /// </summary>
        /// <param name="SourceClientID">ID of the Client who wants to finish his round</param>
        /// <returns></returns>
        public CommandGameEndTurn(string SourceClientID) : base(SourceClientID)
        {
            IsCompleted = false;
        }

        /// <summary>
        /// Set iscompleted on true and the playerremainingmoves to 0
        /// </summary>
        public override void Execute()
        {
            SourcePlayer.PlayerRemainingMoves = 0;

            IsCompleted = true;
        }
    }
}
