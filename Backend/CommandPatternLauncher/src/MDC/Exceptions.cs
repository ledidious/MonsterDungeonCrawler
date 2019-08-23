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

    [Serializable]
    public class CannotConnectToServerException : Exception
    {
        public CannotConnectToServerException()
        {
        }

        public CannotConnectToServerException(string message)
            : base(message)
        {
        }


        public CannotConnectToServerException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    [Serializable]
    public class ClientIsNotConnectedToServerException : Exception
    {
        public ClientIsNotConnectedToServerException()
        {
        }

        public ClientIsNotConnectedToServerException(string message)
            : base(message)
        {
        }

        public ClientIsNotConnectedToServerException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
    [Serializable]
    public class SessionIdIsInvalidException : Exception
    {
        public SessionIdIsInvalidException()
        {
        }

        public SessionIdIsInvalidException(string message)
            : base(message)
        {
        }

        public SessionIdIsInvalidException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}