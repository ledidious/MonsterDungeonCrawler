using Microsoft.VisualStudio.TestTools.UnitTesting;
using MDC.Gamedata;
using MDC.Client;
using MDC.Gamedata.Level;
using MDC.Gamedata.PlayerType;
using MDC.Server;

namespace test
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void NumberOfLife()
        {
            Hero player1 = new Hero("hero", new MeleeFighter(), 20, 30);
            Monster player2 = new Monster("monster", new RangeFighter(), 30, 20);

            Assert.AreEqual(5, player1._life);
            Assert.AreEqual(3, player2._life);
        }

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
        public void AttackEnemy()
        {
            Hero player3 = new Hero("hero", new MeleeFighter(), 20, 30);
            Monster player4 = new Monster("monster", new RangeFighter(), 30, 20);

            player4.DecrementLife(player3.AttackBoost, player3.CharacterType);

            Assert.AreEqual(2, player4._life);
        }

        /*
        //TODO: change Player objects to Monster or Hero objects
        
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
        */

    }
}

