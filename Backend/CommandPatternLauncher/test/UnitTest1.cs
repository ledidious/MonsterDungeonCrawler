using Microsoft.VisualStudio.TestTools.UnitTesting;
using MDC.Gamedata;
using MDC.Client;
using MDC.Gamedata.Level;
using MDC.Gamedata.PlayerType;
using MDC.Server;
using MDC.Exceptions;

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

        [TestMethod]
        [ExpectedException(typeof(CantAttackException))]
        public void AttackMyself()
        {
            CommandManager cm = new CommandManager();
            CommandAttack cattack = new CommandAttack("2f2de19a291c41b5ae950faa11162e07", "2f2de19a291c41b5ae950faa11162e07");

        }
        
        [TestMethod]
        [ExpectedException(typeof(CantAttackException))]
        public void AttackATeammember()
        {
            Monster player5 = new Monster("monster", new RangeFighter(), 30, 20);
            Monster player6 = new Monster("monster", new RangeFighter(), 30, 20);
            CommandManager cm2 = new CommandManager();
            CommandAttack cattack2 = new CommandAttack("2f2de19a291c41b5ae950faa11162e07", "2242342342343");
            cattack2.SourcePlayer = player5; 
            cattack2.TargetPlayer = player6;
            cm2.AddCommand(cattack2);
            cm2.ProcessPendingTransactions();  

        }
        
        [TestMethod]
        public void EnemyInRange()
        {   
            CommandManager cm3 = new CommandManager();

            //Test enemy in range on the right side
            Hero player7 = new Hero("hero", new RangeFighter(), 11, 9);
            Monster player8 = new Monster("monster", new MeleeFighter(), 14, 9);
            CommandAttack cattack3 = new CommandAttack("2f2de19a291c41b5ae950faa11162e07", "8582252885");
            cattack3.SourcePlayer = player7;
            cattack3.TargetPlayer = player8;
           
            Assert.IsTrue(cattack3.VerifyAttackRange());
            
            //Test out of range on the right side
            Monster player9 = new Monster("monster", new MeleeFighter(), 15, 9);
            CommandAttack cattack4 = new CommandAttack("2f2de19a291c41b5ae950faa11162e07", "zut85822asd52885");
            cattack4.SourcePlayer = player7;
            cattack4.TargetPlayer = player9;

            Assert.IsFalse(cattack4.VerifyAttackRange());

            //Test enemy in range on the bottom side
            Monster player10 = new Monster("monster", new MeleeFighter(), 11, 11);
            CommandAttack cattack5 = new CommandAttack("2f2de19a291c41b5ae950faa11162e07", "123124sdf2885");
            cattack5.SourcePlayer = player7;
            cattack5.TargetPlayer = player10;

            Assert.IsTrue(cattack5.VerifyAttackRange());

            //Test out of range on the bottom side
            Monster player11 = new Monster("monster", new MeleeFighter(), 11, 5);
            CommandAttack cattack6 = new CommandAttack("2f2de19a291c41b5ae950faa11162e07", "1sda12423124sdf2885");
            cattack6.SourcePlayer = player7;
            cattack6.TargetPlayer = player11;

            Assert.IsFalse(cattack6.VerifyAttackRange());
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

