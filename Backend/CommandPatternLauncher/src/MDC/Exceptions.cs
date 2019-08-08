using System;

namespace MDC.Exceptions
{

    [Serializable]
    public class CantAttackException : Exception
    {
        public CantAttackException()
           : base("You can't attack yourself or a teammember")
        {
        }
    }

}