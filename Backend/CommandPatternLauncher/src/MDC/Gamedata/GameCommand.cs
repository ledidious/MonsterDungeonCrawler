//Command
using System;
using MDC.Gamedata.PlayerType;

namespace MDC.Gamedata
{
    [Serializable]
    public abstract class GameCommand : Command
    {
        public GameCommand(string SourceClientID) : base(SourceClientID)
        {
        }

        public Player SourcePlayer { get; set; }
    }

    [Serializable]
    public class CommandMove : GameCommand
    {
        // private readonly string _direction;
        private readonly int _moveAmount;

        public CommandMove(string sourceClientID, int moveAmount) : base(sourceClientID)
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
    public class CommandAttack : GameCommand
    {
        public string TargetClientID { get; set; }
        public Player TargetPlayer { get; set; }

        public CommandAttack(string SourceClientID, string targetClientID) : base(SourceClientID)
        {
            TargetClientID = targetClientID;

            if (SourceClientID == TargetClientID)
                throw new Exceptions.CantAttackException();
        }

        public Boolean VerifyAttackRange()
        {
            Boolean TargetInRange = false;

            if (SourcePlayer.XPosition == TargetPlayer.XPosition)
            {
                for (int i = 0; i <= SourcePlayer.CharacterType._attackRange * 2; i++)
                {
                    if (SourcePlayer.YPosition - SourcePlayer.CharacterType._attackRange + i == TargetPlayer.YPosition)
                    {
                        TargetInRange = true;
                    }
                    else
                    {
                        //enemy out of range
                    }
                }

            }

            if (SourcePlayer.YPosition == TargetPlayer.YPosition)
            {
                for (int i = 0; i <= SourcePlayer.CharacterType._attackRange * 2; i++)
                {
                    if (SourcePlayer.XPosition - SourcePlayer.CharacterType._attackRange + i == TargetPlayer.XPosition)
                    {
                        TargetInRange = true;
                    }
                    else
                    {
                        //enemy out of range
                    }

                }

            }


            return TargetInRange;
        }


        //TODO: Mapping ID -> SourceClients for Server :: First identify type of command than execute mapping
        public override void Execute()
        {
            //TODO: Throw Exception if AttackedPlayer not reachable 

            if (SourcePlayer is Monster && TargetPlayer is Monster)
                throw new Exceptions.CantAttackException();

            double attackBoost = SourcePlayer.AttackBoost;
            CharacterType characterType = SourcePlayer.CharacterType;

            TargetPlayer.DecrementLife(attackBoost, characterType);

        }

    }
}
