using Microsoft.VisualStudio.TestTools.UnitTesting;
using MDC.Gamedata;
using MDC.Client;
using MDC.Gamedata.LevelContent;
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

            Assert.AreEqual(5, player1.Life);
            Assert.AreEqual(3, player2.Life);
        }

        [TestMethod]
        [ExpectedException(typeof(System.NullReferenceException), "Object reference not set to an instance of an object.")]
        public void NullTargetPlayerCommand()
        {
            CommandManager cm = new CommandManager();
            CommandGameMove cmove = new CommandGameMove("2f2de19a291c41b5ae950faa11162e07", 5);

            cm.AddCommand(cmove);
            cm.ProcessPendingTransactions();
        }

        [TestMethod]
        public void LevelAddAllFieldObjects()
        {
            Field field1 = new Field(13, 9, new Wall()); 
            Field field8 = new Field(11, 9, new Floor());

            Assert.AreEqual(field1, Level.playingField[13, 9]);
            Assert.AreNotEqual(field1, Level.playingField[10, 9]);

            Assert.AreEqual(field8, Level.playingField[11, 9]);

            Assert.IsInstanceOfType(Level.playingField[13, 9].FieldType, typeof(Wall));
            Assert.IsInstanceOfType(Level.playingField[11, 9].FieldType, typeof(Floor));
        }
    }

    [TestClass]
    public class UnitTestAttack
    {
        [TestMethod]
        public void AttackEnemy()
        {
            Hero player3 = new Hero("hero", new MeleeFighter(), 20, 30);
            Monster player4 = new Monster("monster", new RangeFighter(), 30, 20);

            player4.DecrementLife(player3.AttackBoost, player3.CharacterType);

            Assert.AreEqual(2, player4.Life);
        }

        [TestMethod]
        [ExpectedException(typeof(CantAttackException))]
        public void AttackMyself()
        {
            CommandManager cm = new CommandManager();
            CommandGameAttack cattack = new CommandGameAttack("2f2de19a291c41b5ae950faa11162e07", "2f2de19a291c41b5ae950faa11162e07");

        }
        
        [TestMethod]
        [ExpectedException(typeof(CantAttackException))]
        public void AttackATeammember()
        {
            Monster player5 = new Monster("monster", new RangeFighter(), 30, 20);
            Monster player6 = new Monster("monster", new RangeFighter(), 30, 20);
            CommandManager cm2 = new CommandManager();
            CommandGameAttack cattack2 = new CommandGameAttack("2f2de19a291c41b5ae950faa11162e07", "2242342342343");
            cattack2.SourcePlayer = player5; 
            cattack2.TargetPlayer = player6;
            cm2.AddCommand(cattack2);
            cm2.ProcessPendingTransactions();  

        }
        
        [TestMethod]
        public void EnemyInRange()
        {   
            CommandManager cm3 = new CommandManager();

            //test enemy in range on the right side
            Hero player7 = new Hero("hero", new RangeFighter(), 11, 9);
            Monster player8 = new Monster("monster", new MeleeFighter(), 14, 9);
            CommandGameAttack cattack3 = new CommandGameAttack("2f2de19a291c41b5ae950faa11162e07", "8582252885");
            cattack3.SourcePlayer = player7;
            cattack3.TargetPlayer = player8;
           
            Assert.IsTrue(cattack3.VerifyAttackRange());
            
            //test out of range on the right side
            Monster player9 = new Monster("monster", new MeleeFighter(), 15, 9);
            CommandGameAttack cattack4 = new CommandGameAttack("2f2de19a291c41b5ae950faa11162e07", "zut85822asd52885");
            cattack4.SourcePlayer = player7;
            cattack4.TargetPlayer = player9;

            Assert.IsFalse(cattack4.VerifyAttackRange());

            //test enemy in range on the bottom side
            Monster player10 = new Monster("monster", new MeleeFighter(), 11, 11);
            CommandGameAttack cattack5 = new CommandGameAttack("2f2de19a291c41b5ae950faa11162e07", "123124sdf2885");
            cattack5.SourcePlayer = player7;
            cattack5.TargetPlayer = player10;

            Assert.IsTrue(cattack5.VerifyAttackRange());

            //test out of range on the bottom side
            Monster player11 = new Monster("monster", new MeleeFighter(), 11, 5);
            CommandGameAttack cattack6 = new CommandGameAttack("2f2de19a291c41b5ae950faa11162e07", "1sda12423124sdf2885");
            cattack6.SourcePlayer = player7;
            cattack6.TargetPlayer = player11;

            Assert.IsFalse(cattack6.VerifyAttackRange());
        }



        [TestMethod]
        public void ObstacleInRange()
        {

            //test obstacle in range on the bottom side
            Field field2 = new Field(11, 11, new Wall());
            Field field7 = new Field(11, 10, new Floor()); 

            Hero player12 = new Hero("hero", new MeleeFighter(), 11, 9);
            Monster player13 = new Monster("monster", new MeleeFighter(), 11, 12);

            CommandGameAttack cattack7 = new CommandGameAttack("2f2de19a291c41b5ae950faa11162e07", "1sda12423124sdf2885");
            cattack7.SourcePlayer = player12;
            cattack7.TargetPlayer = player13;

            Assert.IsTrue(cattack7.VerifyObstacleInRange());

            //test obstacle in range on the upper side
            Field field3 = new Field(11, 10, new Wall());

            Hero player14 = new Hero("hero", new MeleeFighter(), 11, 12);
            Monster player15 = new Monster("monster", new MeleeFighter(), 11, 9);

            CommandGameAttack cattack8 = new CommandGameAttack("2f2de19a291c41b5ae950faa111662e07", "1sda1672423124sdf2885");
            cattack8.SourcePlayer = player14;
            cattack8.TargetPlayer = player15;

            Assert.IsTrue(cattack8.VerifyObstacleInRange());

            //test obstacle in range on the right side
            Field field4 = new Field(3, 5, new Wall());

            Hero player16 = new Hero("hero", new MeleeFighter(), 1, 5);
            Monster player17 = new Monster("monster", new MeleeFighter(), 4, 5);

            CommandGameAttack cattack9 = new CommandGameAttack("2f2de19a291c41b5ae950faa111662e07", "1sda1672423124sdf2885");
            cattack9.SourcePlayer = player16;
            cattack9.TargetPlayer = player17;

            Assert.IsTrue(cattack9.VerifyObstacleInRange());

            //test obstacle in range on the left side
            Field field5 = new Field(2, 5, new Wall());

            Hero player18 = new Hero("hero", new MeleeFighter(), 4, 5);
            Monster player19 = new Monster("monster", new MeleeFighter(), 1, 5);

            CommandGameAttack cattack10 = new CommandGameAttack("2f2de19a291c41b5ae950faa111662e07", "1sda1672423124sdf2885");
            cattack10.SourcePlayer = player18;
            cattack10.TargetPlayer = player19;

            Assert.IsTrue(cattack10.VerifyObstacleInRange());
        }

        [TestMethod]
        [ExpectedException(typeof(CantAttackException))]
        public void CantAttackObstacle()
        {
            CommandManager cm4 = new CommandManager();

            //test obstacle in range on the left side
            Field field6 = new Field(2, 5, new Wall());
    
            Hero player19 = new Hero("hero", new MeleeFighter(), 4, 5);
            Monster player20 = new Monster("monster", new MeleeFighter(), 1, 5);

            CommandGameAttack cattack11 = new CommandGameAttack("2f2de19a291c41b5ae950faa111662e07", "1sda1672423124sdf2885");
            cattack11.SourcePlayer = player19;
            cattack11.TargetPlayer = player20;

            cm4.AddCommand(cattack11);
            cm4.ProcessPendingTransactions();         
        }

        [TestMethod]
        [ExpectedException(typeof(CantAttackException))]
        public void CantAttackOutOfRange()
        {

            CommandManager cm5 = new CommandManager();

            Hero player21 = new Hero("hero", new RangeFighter(), 11, 9);
            Monster player22 = new Monster("monster", new MeleeFighter(), 20, 12);
            CommandGameAttack cattack12 = new CommandGameAttack("2f2de19a291c41b5ae950faa11162e07", "8582252885");
            cattack12.SourcePlayer = player21;
            cattack12.TargetPlayer = player22;

            cm5.AddCommand(cattack12);
            cm5.ProcessPendingTransactions();
      
        }
    }
    
    [TestClass]
    public class UnitTestTraps
    {

        [TestMethod]
        public void ManualTrapAttack()
        {
            Hero player1 = new Hero("hero", new RangeFighter(), 11, 9);
            Field field1 = new Field(11, 9, new SpikeField());

            field1.FieldType.Effects(player1);

            Assert.AreEqual(4.5, player1.Life);
            
        }

        [TestMethod]
        public void ManualTrapdoorAttack()
        {

            Hero player2 = new Hero("hero", new RangeFighter(), 11, 9);
            Field field2 = new Field(11, 9, new Trapdoor());

            Field field3 = new Field(5, 5, new Wall());
            Field field4 = new Field(6, 6, new Wall());
            Field field5 = new Field(7, 7, new Floor());

                    
            player2.DecrementLife(0.25);

            if (field2.FieldType is Trapdoor)
            {
                bool successfulMoving = false;
                int randomX = 5; 
                int randomY = 5;

                while (successfulMoving == false)
                {
                    if (Level.playingField[randomX, randomY].FieldType is Floor)
                    {
                        successfulMoving = true;
                        player2.MovePlayer(randomX, randomY);
                    }
                    else
                    {
                        randomX = randomX + 1; 
                        randomY = randomY + 1; 
                    }

                }
            }
            else
            {
                //do not move the player
            }

            Assert.AreEqual(4.75, player2.Life);
            Assert.AreEqual(7, player2.XPosition);
            Assert.AreEqual(7, player2.YPosition);

        }       
    }
        



        //TODO: test if player can access a floor object and a wall object        

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

    

    [TestClass]
    public class UnitTestItems
    {

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void ItemLevelInvalidLevel()
        {
            Field field8 = new Field(11, 11, new Floor());
            field8.Item = new DefenseBoost(8); 
        }
        
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void ItemAddedToInvalidFieldtype()
        {
            Field field9 = new Field(11, 19, new Wall());
            field9.Item = new DefenseBoost(2); 
        }

        [TestMethod]
        public void ItemDurationAndEffectValue()
        {
            Field field10 = new Field(19, 19, new Floor());
            field10.Item = new ExtraLife(); 

            Assert.AreEqual(1, field10.Item.EffectValue);


            Field field11 = new Field(15, 19, new Floor());
            field11.Item = new DefenseBoost(2); 

            Assert.AreEqual(0.5, field11.Item.EffectValue);
            Assert.AreEqual(3, field11.Item.Duration);


            Field field12 = new Field(5, 9, new Floor());
            field12.Item = new AttackBoost(3); 

            Assert.AreEqual(0.75, field12.Item.EffectValue);
            Assert.AreEqual(3, field12.Item.Duration);


        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void CantSetDurationForExtralife()
        {
            Field field13 = new Field(1, 19, new Floor());
            field13.Item = new ExtraLife();

            field13.Item.Duration = 13; 
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void ExtralifeHasNoDuration()
        {
            Field field14 = new Field(11, 1, new Floor());
            field14.Item = new ExtraLife();

            int test = field14.Item.Duration; 
        }

        






    }

}

