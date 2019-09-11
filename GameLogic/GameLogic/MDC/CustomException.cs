using System;

namespace GameLogic.MDC
{
    // ###############
    // # Server side #
    // ###############
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

        protected CantAttackException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
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

        protected CantMoveException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
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

        protected SessionIdIsInvalidException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
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

        protected NotEnoughPlayerInGameException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class GameLobbyIsFullException : Exception
    {
        public GameLobbyIsFullException()
        {
        }

        public GameLobbyIsFullException(string message)
            : base(message)
        {
        }

        public GameLobbyIsFullException(string message, Exception inner)
            : base(message, inner)
        {
        }

        // Constructor needed for serialization 
        // when exception propagates from a remoting server to the client.
        protected GameLobbyIsFullException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class CommandNotRecognizedException : Exception
    {
        public CommandNotRecognizedException()
        {
        }

        public CommandNotRecognizedException(string message)
            : base(message)
        {
        }

        public CommandNotRecognizedException(string message, Exception inner)
            : base(message, inner)
        {
        }

        // Constructor needed for serialization 
        // when exception propagates from a remoting server to the client.
        protected CommandNotRecognizedException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    // ##########
    // # Client #
    // ##########
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
}
