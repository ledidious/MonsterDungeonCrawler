using System.Collections.Generic;
using System.Linq;
using GameLogic.MDC.Gamedata;

namespace GameLogic.MDC.Server
{
    public class CommandManager
    {
        private readonly List<Command> _transactions = new List<Command>();

        public bool HasPendingCommands
        {
            get { return _transactions.Any(x => !x.IsCompleted); }
        }

        /// <summary>
        /// Add new commands to the list
        /// </summary>
        /// <param name="transaction">The command to add</param>
        public void AddCommand(Command transaction)
        {
            _transactions.Add(transaction);
        }

        /// <summary>
        /// Execute all commands in the list (that have not yet been successfully executed) in the order they were added.
        /// </summary>
        public void ProcessPendingTransactions()
        {
            foreach (Command command in _transactions.Where(x => !x.IsCompleted))
            {
                try
                {
                    command.Execute();
                }
                catch (System.Exception)
                {
                    throw;
                }
            }
        }
    }
}
