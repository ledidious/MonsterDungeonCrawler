using System;

namespace MDC.Exceptions
{

    [Serializable]
    public class CantAttackException : Exception
    {
        public CantAttackException()
        {
        }

        public CantAttackException(string message)
            : base(message)
        {
        }


        public CantAttackException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }


    [Serializable]
    public class CantMoveException : Exception
    {
        public CantMoveException()
        {
        }

        public CantMoveException(string message)
            : base(message)
        {
        }


        public CantMoveException(string message, Exception inner)
            : base(message, inner)
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

    public class NotEnoughPlayerInGameException : Exception
    {
        public NotEnoughPlayerInGameException()
        {
        }

        public NotEnoughPlayerInGameException(string message)
            : base(message)
        {
        }

        public NotEnoughPlayerInGameException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}