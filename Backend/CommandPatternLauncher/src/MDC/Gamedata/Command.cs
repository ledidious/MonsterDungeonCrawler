//Command
using System;
using MDC.Gamedata.PlayerType;

namespace MDC.Gamedata
{
    [Serializable]
    public abstract class Command
    {
        public Player TargetPlayer { get; set; }
        public string ClientID { get; set; }
        public bool IsCompleted { set; get; }
        abstract public void Execute();
    }

    [Serializable]
    public class CommandMove : Command
    {
        // private readonly string _direction;
        private readonly int _moveAmount;

        public CommandMove(string clientID, int moveAmount)
        {
            ClientID = clientID;
            _moveAmount = moveAmount;
            // _direction = direction;

            IsCompleted = false;
        }

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
