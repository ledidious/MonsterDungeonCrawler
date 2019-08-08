//Command
using System;
using MDC.Gamedata.PlayerType;

namespace MDC.Gamedata
{
    [Serializable]
    public abstract class GameCommand : Command
    {
        public GameCommand(string clientID) : base(clientID)
        {
        }

        public Player TargetPlayer { get; set; }
    }

    [Serializable]
    public class CommandMove : GameCommand
    {
        // private readonly string _direction;
        private readonly int _moveAmount;

        public CommandMove(string clientID, int moveAmount) : base(clientID)
        {
            _moveAmount = moveAmount;
            IsCompleted = false;
        }

        // public CommandMove(string clientID, int moveAmount)
        // {
        //     ClientID = clientID;
        //     _moveAmount = moveAmount;
        //     // _direction = direction;

        //     IsCompleted = false;
        // }

        /// <summary>
        /// set IsComplete on true if the TargetPlayer has more or equal remaining moves than the Input (_moveAmount)
        /// </summary>
        public override void Execute()
        {
            try
            {
                if (TargetPlayer.PlayerRemainingMoves >= _moveAmount)
                {
                    TargetPlayer.PlayerRemainingMoves -= _moveAmount;

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

}
