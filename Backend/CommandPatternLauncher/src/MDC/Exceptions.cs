using System;

namespace MDC.Exceptions
{

    [Serializable]
    public class CantAttackException : Exception
    {
        public CantAttackException()
           : base("You can't attack yourself, a teammember or an enemy out of range!")
        {
        }
    }


    [Serializable]
    public class CantMoveException : Exception
    {
        public CantMoveException()
           : base("Invalid move!")
        {
        }
    }

}