using Microsoft.VisualStudio.TestTools.UnitTesting;
using MDC.Gamedata;
using MDC.Client;
using MDC.Gamedata.LevelContent;
using MDC.Gamedata.PlayerType;
using MDC.Server;
using MDC.Exceptions;

namespace MonsterdungeonCrawlerTests
{

    [TestClass]
    public class UnitTestGeneral
    {

        [TestMethod]
        public void NumberOfLife()
        {
            Hero player1 = new Hero("hero", new MeleeFighter(), 10, 10);
            Monster player2 = new Monster("monster", new RangeFighter(), 10, 19);

            Assert.AreEqual(5, player1.Life);
            Assert.AreEqual(3, player2.Life);
        }

        /// <summary>
        /// move 2 fields (trap, item) and then attack enemy
        /// </summary>
        [TestMethod]
        public void GameSzenario1()
        {   //need number of max. clients

            Level.playerList.Clear();
            Level.trapList.Clear();

            Hero player3 = new Hero("hero", new MeleeFighter(), 10, 18); 
            Monster player4 = new Monster("monster", new RangeFighter(), 8, 16);
            Monster player5 = new Monster("monster", new MeleeFighter(), 13, 1);   
            Monster player6 = new Monster("monster", new RangeFighter(), 2, 2);  
            Field field1 = new Field(10, 17, new Floor());
            Field field2 = new Field(9, 17, new SpikeField());
            Field field3 = new Field(8, 17, new Floor());
            field3.Item = new AttackBoost(3); 

            CommandManager cm1 = new CommandManager(); 


            //move to field (10, 17)
            CommandGameMove cmove1 = new CommandGameMove("adua5as7da5sd5", 10, 17); 
            cmove1.SourcePlayer = player3; 
            cm1.AddCommand(cmove1); 
            cm1.ProcessPendingTransactions(); 

            Assert.AreEqual(4, player3.PlayerRemainingMoves);
            Assert.IsTrue(10 == player3.XPosition && 17 == player3.YPosition); 


            //move to field with trap (9, 17)
            CommandGameMove cmove2 = new CommandGameMove("adua5as7da5sd5", 9, 17); 
            cmove2.SourcePlayer = player3; 
            cm1.AddCommand(cmove2); 
            cm1.ProcessPendingTransactions();   

            Assert.AreEqual(3, player3.PlayerRemainingMoves);
            Assert.IsTrue(9 == player3.XPosition && 17 == player3.YPosition);
            Assert.AreEqual(4.75, player3.Life);


            //move to field with item (8, 17)
            CommandGameMove cmove3 = new CommandGameMove("adua5as7da5sd5", 8, 17); 
            cmove3.SourcePlayer = player3; 
            cm1.AddCommand(cmove3); 
            cm1.ProcessPendingTransactions();   

            Assert.AreEqual(2, player3.PlayerRemainingMoves);
            Assert.IsTrue(8 == player3.XPosition && 17 == player3.YPosition);
            Assert.AreEqual(0.75, player3.AttackBoost);


            //attack monster (8, 16)
            CommandGameAttack cattack1 = new CommandGameAttack("adua5as7da5sd5", "6a6sd465a4s9"); 
            cattack1.SourcePlayer = player3; 
            cattack1.TargetPlayer = player4; 
            cm1.AddCommand(cattack1); 
            cm1.ProcessPendingTransactions();  

            Assert.AreEqual(0, player3.PlayerRemainingMoves);
            Assert.IsTrue(8 == player3.XPosition && 17 == player3.YPosition);
            Assert.IsTrue(8 == player4.XPosition && 16 == player4.YPosition);
            Assert.AreEqual(1.25, player4.Life);
        } 
    }

    [TestClass]
    public class UnitTestLevelField
    {
        [TestMethod]
        public void LevelAddAllFieldObjects()
        {
            Level.playerList.Clear();
            Level.trapList.Clear(); 

            Field field1 = new Field(13, 9, new Wall()); 
            Field field8 = new Field(11, 9, new Floor());

            Assert.AreEqual(field1, Level.playingField[13, 9]);
            Assert.AreNotEqual(field1, Level.playingField[10, 9]);

            Assert.AreEqual(field8, Level.playingField[11, 9]);

            Assert.IsInstanceOfType(Level.playingField[13, 9].FieldType, typeof(Wall));
            Assert.IsInstanceOfType(Level.playingField[11, 9].FieldType, typeof(Floor));
        }

        [TestMethod]
        public void LevelAddTrapToTrapList()
        {
            Level.playerList.Clear();
            Level.trapList.Clear(); 

            Field field2 = new Field(3, 9, new Wall()); 
            Field field3 = new Field(1, 9, new Floor());           
            Field field4 = new Field(11, 8, new SpikeField()); 
            Field field5 = new Field(8, 8, new LaserBeam()); 

            Assert.AreEqual(2, Level.trapList.Count);

        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void PlayerPositionOutOfField()
        {
            Level.playerList.Clear();
            Level.trapList.Clear();

            Hero player1 = new Hero("hero", new MeleeFighter(), 19, 30);
        }

        [TestMethod]
        public void LevelFieldBlockedByPlayer()
        {
            Level.playerList.Clear();
            Level.trapList.Clear();
             
            Hero player1 = new Hero("hero", new MeleeFighter(), 19, 19);
            Monster player2 = new Monster("monster", new RangeFighter(), 10, 19); 
            Monster player3 = new Monster("monster", new RangeFighter(), 11, 19);
            
        
            Assert.IsTrue(Level.FieldBlockedByPlayer(11, 19)); 
            Assert.IsTrue(Level.FieldBlockedByPlayer(19, 19));
            Assert.IsTrue(Level.FieldBlockedByPlayer(10, 19));
            Assert.IsFalse(Level.FieldBlockedByPlayer(10, 3));
            Assert.IsFalse(Level.FieldBlockedByPlayer(10, 9));
            Assert.IsFalse(Level.FieldBlockedByPlayer(7, 7));

            Assert.AreEqual(3, Level.playerList.Count);

        }
    }

    [TestClass]
    public class UnitTestMove
    {
        /*
        [TestMethod]
        [ExpectedException(typeof(System.NullReferenceException), "Object reference not set to an instance of an object.")]
        public void NullTargetPlayerCommand()
        {
            CommandManager cm = new CommandManager();
            CommandGameMove cmove = new CommandGameMove("2f2de19a291c41b5ae950faa11162e07", 5);

            cm.AddCommand(cmove);
            cm.ProcessPendingTransactions();
        }
        */

        [TestMethod]
        public void InvalidTargetField()
        {
            Level.playerList.Clear();
            Level.trapList.Clear();

            //targetfield is equal current field
            Hero player3 = new Hero("hero", new MeleeFighter(), 18, 18);
            Field field1 = new Field(18, 18, new Floor());

            CommandGameMove cmove1 = new CommandGameMove("2f2de19a291c41b5ae950faa11162e07", 18, 18);
            cmove1.SourcePlayer = player3;  

            Assert.IsTrue(cmove1.TargetFieldIsInvalid()); 


            //targetfield is a wall
            Hero player4 = new Hero("hero", new MeleeFighter(), 8, 8);
            Field field2 = new Field(8, 9, new Wall());      

            CommandGameMove cmove2 = new CommandGameMove("2f2de19a291c41b5ae950faa11162e07",8, 9);      
            cmove2.SourcePlayer = player4; 

            Assert.IsTrue(cmove2.TargetFieldIsInvalid());


            //targetfield is blocked by another player
            Hero player5 = new Hero("hero", new MeleeFighter(), 2, 2);
            Monster player6 = new Monster("monster", new RangeFighter(), 2, 3);
            Field field3 = new Field(2, 3, new Floor());

            CommandGameMove cmove3 = new CommandGameMove("2f2de19a291c41b5ae950faa11162e07",2, 3);      
            cmove3.SourcePlayer = player5;       

            Assert.IsTrue(cmove3.TargetFieldIsInvalid());


            //targetfield is valid
            Hero player7 = new Hero("hero", new MeleeFighter(), 1, 1);
            Field field4 = new Field(1, 2, new Floor());

            CommandGameMove cmove4 = new CommandGameMove("2f2de19a291c41b5ae950faa11162e07", 1, 2);      
            cmove4.SourcePlayer = player7;       

            Assert.IsFalse(cmove4.TargetFieldIsInvalid());

            CommandManager cm1 = new CommandManager(); 
            cm1.AddCommand(cmove4);
            cm1.ProcessPendingTransactions();

            Assert.AreEqual(4, player7.PlayerRemainingMoves);


            //targetfield is diagonal
            Hero player8 = new Hero("hero", new MeleeFighter(), 5, 5);
            Field field5 = new Field(6, 6, new Floor());

            CommandGameMove cmove5 = new CommandGameMove("5a45dsf5s4as5d4as8", 6, 6);   
            cmove5.SourcePlayer = player8; 

            Assert.IsFalse(cmove5.VerifyMoveRange()); 
        }

        [TestMethod]
        public void MoveRangeCharactertypes()
        {
            Level.playerList.Clear();
            Level.trapList.Clear();

            Hero player1 = new Hero("hero", new MeleeFighter(), 18, 18);
            Monster player2 = new Monster("monster", new RangeFighter(), 19, 18);

            Assert.AreEqual(5, player1.CharacterType._moveRange);
            Assert.AreEqual(2, player2.CharacterType._moveRange);

            Assert.AreEqual(5, player1.PlayerRemainingMoves); 
        }



        [TestMethod]
        public void TrapTargetField()
        {   //need number of max. clients

            Level.playerList.Clear();
            Level.trapList.Clear();

            Monster player8 = new Monster("monster", new RangeFighter(), 2, 3);
            Monster player9 = new Monster("monster", new RangeFighter(), 3, 3);
            Monster player10 = new Monster("monster", new RangeFighter(), 4, 3);

            Hero player11 = new Hero("hero", new MeleeFighter(), 10, 10);
            Field field5 = new Field(10, 11, new SpikeField());
            Field field6 = new Field(10, 12, new LaserBeam());

            field6.FieldType.OnNextRound(); 
              
            CommandGameMove cmove5 = new CommandGameMove("2f2de19a291c41b5ae950faa11162e07",10, 11);
            cmove5.SourcePlayer = player11;

            CommandManager cm2 = new CommandManager(); 
            cm2.AddCommand(cmove5);
            cm2.ProcessPendingTransactions();

            Assert.AreEqual(4.75, player11.Life);

            CommandGameMove cmove6 = new CommandGameMove("2f2de19a291c41b5ae950faa11162e07",10, 12);
            cmove6.SourcePlayer = player11; 
            cm2.AddCommand(cmove6);
            cm2.ProcessPendingTransactions();

            Assert.AreEqual(4, player11.Life);
        }

        [TestMethod]
        public void MoveInRange()
        {   
            Level.playerList.Clear();
            Level.trapList.Clear();

            Hero player12 = new Hero("hero", new MeleeFighter(), 16, 12);

            //move down
            CommandGameMove cmove7 = new CommandGameMove("54s56s45df66s",16, 13);
            cmove7.SourcePlayer = player12; 

            Assert.IsTrue(cmove7.VerifyMoveRange()); 

            //move up
            CommandGameMove cmove8 = new CommandGameMove("54s56s45df66s",16, 11);
            cmove8.SourcePlayer = player12; 

            Assert.IsTrue(cmove8.VerifyMoveRange());

            //move right
            CommandGameMove cmove9 = new CommandGameMove("54s56s45df66s",17, 12);
            cmove9.SourcePlayer = player12; 

            Assert.IsTrue(cmove9.VerifyMoveRange());

            //move left
            CommandGameMove cmove10 = new CommandGameMove("54s56s45df66s",15, 12);
            cmove10.SourcePlayer = player12; 

            Assert.IsTrue(cmove10.VerifyMoveRange());

            //move out of range
            CommandGameMove cmove11 = new CommandGameMove("54s56s45df66s",16, 14);
            cmove11.SourcePlayer = player12; 

            Assert.IsFalse(cmove11.VerifyMoveRange()); 

            //move out of range
            CommandGameMove cmove12 = new CommandGameMove("54s56s45df66s",18, 12);
            cmove12.SourcePlayer = player12; 

            Assert.IsFalse(cmove12.VerifyMoveRange()); 

            //move out of range
            CommandGameMove cmove13 = new CommandGameMove("54s56s45df66s",18, 13);
            cmove13.SourcePlayer = player12; 

            Assert.IsFalse(cmove13.VerifyMoveRange()); 
        }

        [TestMethod]
        public void ItemOnTargetField()
        {   //need number of max. clients

            Level.playerList.Clear();
            Level.trapList.Clear();

            Monster player13 = new Monster("monster", new RangeFighter(), 2, 3);
            Monster player14 = new Monster("monster", new RangeFighter(), 3, 3);
            Monster player15 = new Monster("monster", new RangeFighter(), 4, 3);

            Hero player16 = new Hero("hero", new MeleeFighter(), 10, 10);
            Field field7 = new Field(10, 11, new Floor());
            Field field8 = new Field(10, 13, new Floor());
            Field field9 = new Field(10, 12, new Floor());

            field7.Item = new DefenseBoost(2); 
            field9.Item = new AttackBoost(3); 

            CommandGameMove cmove14 = new CommandGameMove("234hug2haa1248325sdf5",10, 11);
            cmove14.SourcePlayer = player16; 

            CommandManager cm3 = new CommandManager(); 
            cm3.AddCommand(cmove14);
            cm3.ProcessPendingTransactions();

            Assert.AreEqual(0.5, player16.DefenseBoost);
            Assert.AreEqual(null, field7.Item); 
            Assert.AreEqual(null, field8.Item); 

            CommandGameMove cmove16 = new CommandGameMove("234hug2haa1248325sdf5",10, 12);
            cmove16.SourcePlayer = player16; 

            cm3.AddCommand(cmove16); 
            cm3.ProcessPendingTransactions();

            Assert.AreEqual(0.75, player16.AttackBoost);
            Assert.AreEqual(null, field9.Item); 
        }

    }

    [TestClass]
    public class UnitTestAttack
    {
        [TestMethod]
        public void AttackEnemy()
        {
            Hero player3 = new Hero("hero", new MeleeFighter(), 18, 18);
            Monster player4 = new Monster("monster", new RangeFighter(), 19, 18);

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
            Monster player5 = new Monster("monster", new RangeFighter(), 10, 7);
            Monster player6 = new Monster("monster", new RangeFighter(), 10, 7);
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
        public void ObstacleInRange1()
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
            Monster player22 = new Monster("monster", new MeleeFighter(), 19, 12);
            CommandGameAttack cattack12 = new CommandGameAttack("2f2de19a291c41b5ae950faa11162e07", "8582252885");
            cattack12.SourcePlayer = player21;
            cattack12.TargetPlayer = player22;

            cm5.AddCommand(cattack12);
            cm5.ProcessPendingTransactions();
        }

        [TestMethod]
        [ExpectedException(typeof(CantAttackException))]
        public void AttackWithoutRemainingMoves()
        {   //need number of max. clients

            Level.playerList.Clear();
            Level.trapList.Clear();

            Hero player3 = new Hero("hero", new MeleeFighter(), 10, 18); 
            Monster player4 = new Monster("monster", new RangeFighter(), 8, 14);
            Monster player5 = new Monster("monster", new MeleeFighter(), 13, 1);   
            Monster player6 = new Monster("monster", new RangeFighter(), 2, 2);  
            Field field1 = new Field(10, 17, new Floor());
            Field field2 = new Field(9, 17, new SpikeField());
            Field field3 = new Field(8, 17, new Floor());
            Field field4 = new Field(8, 16, new Floor());
            Field field5 = new Field(8, 15, new Floor());
            field3.Item = new AttackBoost(3); 

            CommandManager cm1 = new CommandManager(); 

            //move to field (10, 17)
            CommandGameMove cmove1 = new CommandGameMove("adua5as7da5sd5", 10, 17); 
            cmove1.SourcePlayer = player3; 
            cm1.AddCommand(cmove1); 
            cm1.ProcessPendingTransactions(); 

            //move to field with trap (9, 17)
            CommandGameMove cmove2 = new CommandGameMove("adua5as7da5sd5", 9, 17); 
            cmove2.SourcePlayer = player3; 
            cm1.AddCommand(cmove2); 
            cm1.ProcessPendingTransactions();   

            //move to field with item (8, 17)
            CommandGameMove cmove3 = new CommandGameMove("adua5as7da5sd5", 8, 17); 
            cmove3.SourcePlayer = player3; 
            cm1.AddCommand(cmove3); 
            cm1.ProcessPendingTransactions(); 

            //move to field with item (8, 16)
            CommandGameMove cmove4 = new CommandGameMove("adua5as7da5sd5", 8, 16); 
            cmove4.SourcePlayer = player3; 
            cm1.AddCommand(cmove4); 
            cm1.ProcessPendingTransactions();

            //move to field with item (8, 15)
            CommandGameMove cmove5 = new CommandGameMove("adua5as7da5sd5", 8, 15); 
            cmove5.SourcePlayer = player3; 
            cm1.AddCommand(cmove5); 
            cm1.ProcessPendingTransactions();

            //attack monster (8, 14)
            CommandGameAttack cattack1 = new CommandGameAttack("adua5as7da5sd5", "6a6sd465a4s9"); 
            cattack1.SourcePlayer = player3; 
            cattack1.TargetPlayer = player4; 
            cm1.AddCommand(cattack1); 
            cm1.ProcessPendingTransactions();
        }

        [TestMethod]
        [ExpectedException(typeof(CantAttackException))]
        public void ObstacleInRange2()
        {   //need number of max. clients

            Level.playerList.Clear();
            Level.trapList.Clear();

            Hero player7 = new Hero("hero", new MeleeFighter(), 1, 5); 
            Monster player8 = new Monster("monster", new RangeFighter(), 4, 5);
            Monster player9 = new Monster("monster", new MeleeFighter(), 13, 1);   
            Monster player10 = new Monster("monster", new RangeFighter(), 2, 2);  
            Field field1 = new Field(2, 5, new Wall());
            Field field2 = new Field(3, 4, new Floor());

            CommandManager cm2 = new CommandManager(); 

            CommandGameAttack cattack1 = new CommandGameAttack("adua5as7da5sd5", "6a6sd465a4s9"); 
            cattack1.SourcePlayer = player7; 
            cattack1.TargetPlayer = player8; 
            cm2.AddCommand(cattack1); 
            cm2.ProcessPendingTransactions();
        }

        [TestMethod]
        public void ObstacleInRange3()
        {   //need number of max. clients

            Level.playerList.Clear();
            Level.trapList.Clear();

            Hero player11 = new Hero("hero", new RangeFighter(), 3, 13); 
            Monster player12 = new Monster("monster", new RangeFighter(), 4, 13);
            Monster player13 = new Monster("monster", new MeleeFighter(), 13, 1);   
            Monster player14 = new Monster("monster", new RangeFighter(), 15, 2);  
            Field field1 = new Field(4, 13, new Floor());
            Field field2 = new Field(3, 12, new Wall());
            Field field3 = new Field(2, 13, new Wall());
            Field field4 = new Field(3, 14, new Wall());

            CommandManager cm3 = new CommandManager(); 

            CommandGameAttack cattack2 = new CommandGameAttack("adua5as7da5sd5", "6a6sd465a4s9"); 
            cattack2.SourcePlayer = player11; 
            cattack2.TargetPlayer = player12; 
            cm3.AddCommand(cattack2); 
            cm3.ProcessPendingTransactions();
        }
    }
    
    [TestClass]
    public class UnitTestTraps
    {

        [TestMethod]
        public void ManualTrapAttack()
        {
            Level.playerList.Clear();
            Level.trapList.Clear();

            Hero player1 = new Hero("hero", new RangeFighter(), 11, 9);
            Field field1 = new Field(11, 9, new SpikeField());

            field1.FieldType.Effects(player1);

            Assert.AreEqual(4.5, player1.Life);
            
        }

        [TestMethod]
        public void ManualTrapdoorAttack()
        {
            Level.playerList.Clear();
            Level.trapList.Clear();

            Hero player2 = new Hero("hero", new RangeFighter(), 11, 9);
            Monster player22 = new Monster("monster", new MeleeFighter(), 7, 7);
            Field field2 = new Field(11, 9, new Trapdoor());

            Field field3 = new Field(5, 5, new Wall());
            Field field4 = new Field(6, 6, new Wall());
            Field field5 = new Field(7, 7, new Floor());
            Field field6 = new Field(8, 8, new Floor());
 
                    
            player2.DecrementLife(0.25);

            if (field2.FieldType is Trapdoor)
            {
                bool successfulMoving = false;
                int randomX = 5; 
                int randomY = 5;

                while (successfulMoving == false)
                {
                    if (Level.playingField[randomX, randomY].FieldType is Floor && Level.FieldBlockedByPlayer(randomX, randomY) == false)
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
            Assert.AreEqual(8, player2.XPosition);
            Assert.AreEqual(8, player2.YPosition);

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
            Level.playerList.Clear();
            Level.trapList.Clear();

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
            //Assert.AreEqual(3, field11.Item.Duration);


            Field field12 = new Field(5, 9, new Floor());
            field12.Item = new AttackBoost(3); 

            Assert.AreEqual(0.75, field12.Item.EffectValue);
            //Assert.AreEqual(3, field12.Item.Duration);

        }
        /*
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
        */

        [TestMethod]
        public void PlayerCollectItem()
        {
            Hero player1 = new Hero("hero", new RangeFighter(), 11, 1);
            Field field15 = new Field(11, 1, new Floor());
            field15.Item = new DefenseBoost(2);
            
            player1.CollectItem(field15.Item); 
            
            Assert.AreEqual(0.5, player1.DefenseBoost); 

            player1.ResetBoost(); 

            Assert.AreEqual(0, player1.DefenseBoost);
            Assert.AreEqual(0, player1.AttackBoost);

        
            Monster player2 = new Monster("monster", new MeleeFighter(), 7, 7);
            Field field16 = new Field(7, 7, new Floor());
            field16.Item = new DefenseBoost(2);

            player2.CollectItem(field16.Item); 
            
            Assert.AreEqual(0.5, player2.DefenseBoost); 

            player2.ResetBoost();

            Assert.AreEqual(0, player2.DefenseBoost);
            Assert.AreEqual(0, player2.AttackBoost);
            
        } 

        /*
        [TestMethod]
        [ExpectedException(typeof(System.ArgumentException))]
        public void ItemOutOfRange()
        {
            Monster player3 = new Monster("monster", new MeleeFighter(), 6, 7);
            Field field17 = new Field(7, 7, new Floor());
            Field field18 = new Field(6, 7, new Floor());
            field17.Item = new DefenseBoost(2);

            player3.CollectItem(field17.Item);  
        }
        */

        [TestMethod]
        public void IgnoreExtralife()
        {
            Hero player4 = new Hero("hero", new RangeFighter(), 11, 1);
            Field field19 = new Field(11, 1, new Floor());
            field19.Item = new ExtraLife(); 

            Assert.IsFalse(player4.CollectItem(field19.Item)); 

            player4.Life--; 

            Assert.IsTrue(player4.CollectItem(field19.Item)); 

        }

        [TestMethod]
        public void DeleteBooster()
        {
            Hero player5 = new Hero("hero", new RangeFighter(), 12, 1);
            Field field20 = new Field(12, 1, new Floor());
            field20.Item = new DefenseBoost(2); 

            Assert.IsTrue(player5.CollectItem(field20.Item));

            Assert.IsTrue(Level.playerList[0].DefenseItem.DecrementBoosterDuration());
            Assert.IsTrue(Level.playerList[0].DefenseItem.DecrementBoosterDuration());
            Assert.IsFalse(Level.playerList[0].DefenseItem.DecrementBoosterDuration());


            Level.playerList[0].DefenseItem = null; 

        }

    }

}

