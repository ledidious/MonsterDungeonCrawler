using GameLogic.MDC.Gamedata.PlayerType;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
        }
    }

    [TestClass]
    public class UnitTestGeneral
    {
        public const int MAX_PLAYER = 4;

        [TestMethod]
        public void NumberOfLife()
        {
            Hero player1 = new Hero("hero", new Knight(), 10, 10);
            Monster player2 = new Monster("monster", new Archer(), 10, 19);

            Assert.AreEqual(5, player1.Life);
            Assert.AreEqual(3, player2.Life);
        }
    }
}
