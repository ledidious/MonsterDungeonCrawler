using System;
using GameLogic.MDC.Server;
using GameLogic.MDC.Client;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("[1] Server\n[2] Simulate Client\n[3] Test Stuff");
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    MasterServer.StartServer();
                    break;
                case "2":
                    Console.WriteLine("Please insert SessionID");
                    string s_ID = Console.ReadLine();
                    ClientProgram gameClient = new ClientProgram();
                    gameClient.ConnectToServer();
                    System.Threading.Thread.Sleep(500);
                    gameClient.ConnectToGame(s_ID);
                    System.Threading.Thread.Sleep(500);
                    gameClient.CreateNewPlayerForSession("TestClient", GameLogic.MDC.CharacterClass.Knight);
                    break;
                case "3":
                    // MasterServer.StartServer();
                    // System.Threading.Thread.Sleep(500);

                    ClientProgram client01 = new ClientProgram();
                    ClientProgram client02 = new ClientProgram();
                    ClientProgram client03 = new ClientProgram();
                    ClientProgram client04 = new ClientProgram();

                    client01.ConnectToServer();
                    client02.ConnectToServer();
                    client03.ConnectToServer();
                    client04.ConnectToServer();

                    client01.CreateNewGame("level1");
                    client02.ConnectToGame(client01.GameSession_ID);
                    client03.ConnectToGame(client01.GameSession_ID);
                    client04.ConnectToGame(client01.GameSession_ID);

                    client01.CreateNewPlayerForSession("A", GameLogic.MDC.CharacterClass.Knight);
                    client02.CreateNewPlayerForSession("B", GameLogic.MDC.CharacterClass.Archer);
                    client03.CreateNewPlayerForSession("C", GameLogic.MDC.CharacterClass.Knight);
                    client04.CreateNewPlayerForSession("D", GameLogic.MDC.CharacterClass.Archer);

                    System.Threading.Thread.Sleep(3500);
                    client01.StartCreatedGame();

                    System.Threading.Thread.Sleep(3500);

                    client01.MovePlayer(2, 1);
                    client01.MovePlayer(1, 1);
                    client01.MovePlayer(2, 1);
                    client01.MovePlayer(3, 1);
                    client01.MovePlayer(4, 1);



                    while (true)
                    {

                    }
                    break;
                default:
                    Console.WriteLine("No input detected!");
                    break;
            }

            while (true)
            {

            }

            /*
            ClientProgram myFirstClient = new ClientProgram();

            //Thread serverThread = new Thread(new ThreadStart(() => MasterServer.StartServer()));
            //serverThread.Start();



            System.Threading.Thread.Sleep(500);

            myFirstClient.ConnectToServer();
            myFirstClient.CreateNewGame("level1");
            myFirstClient.CreateNewPlayerForSession("Snoop Dog", GameLogic.MDC.CharacterClass.Knight);




            // myFirstClient.DisconnectFromServer();

            System.Threading.Thread.Sleep(500);

            ClientProgram mySecondClient = new ClientProgram();
            mySecondClient.ConnectToServer();
            mySecondClient.ConnectToGame(myFirstClient.GameSession_ID);
            mySecondClient.CreateNewPlayerForSession("Tux", GameLogic.MDC.CharacterClass.Archer);

            myFirstClient.GetUpdatePackForLobby();

            while (true)
            {
                if (myFirstClient.Update != null)
                {
                    Console.WriteLine("C01: " + myFirstClient.Update.PlayerList[0].PlayerName);
                    Console.WriteLine("C01: " + myFirstClient.Update.PlayerList[1].PlayerName);
                }
                else
                {
                    System.Threading.Thread.Sleep(1500);
                }
            } */
        }
    }
}
