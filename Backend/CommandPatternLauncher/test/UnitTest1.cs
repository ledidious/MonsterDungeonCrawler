using Microsoft.VisualStudio.TestTools.UnitTesting;
using MDC.Server;
using MDC.Gamedata;
using MDC.Client;

namespace test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        [ExpectedException(typeof(System.NullReferenceException), "Object reference not set to an instance of an object.")]
        public void NullTargetPlayerCommand()
        {
            CommandManager cm = new CommandManager();
            CommandMove cmove = new CommandMove("2f2de19a291c41b5ae950faa11162e07", 5);

            cm.AddCommand(cmove);
            cm.ProcessPendingTransactions();
        }

        [TestMethod]
        public void TestAllCommandsSuccessful()
        {
            CommandManager cm = new CommandManager();

            //---try assigning an invalid value.--- 
            Player cheatingPlayer = new Player("Cheater", 42);
            Assert.AreEqual(-999, cheatingPlayer.PlayerRemainingMoves);

            //---assigning a correct value.--- 
            Player correctPlayer = new Player("Borsti", 20);
            Assert.AreEqual(20, correctPlayer.PlayerRemainingMoves);

            CommandMove cmove = new CommandMove("2f2de19a291c41b5ae950faa11162e07", 5);

            cmove.TargetPlayer = correctPlayer;
            cm.AddCommand(cmove);

            Assert.IsTrue(cm.HasPendingCommands);

            cm.ProcessPendingTransactions();
            Assert.AreEqual(15, correctPlayer.PlayerRemainingMoves);
        }

       }
}
