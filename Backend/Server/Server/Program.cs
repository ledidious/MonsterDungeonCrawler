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

                    client01.ConnectToServer();
                    client02.ConnectToServer();

                    Console.WriteLine("Please enter session id");
                    string sessionID = Console.ReadLine();
                    client01.ConnectToGame(sessionID);
                    client02.ConnectToGame(sessionID);

                    client01.CreateNewPlayerForSession("C", GameLogic.MDC.CharacterClass.Knight);
                    client02.CreateNewPlayerForSession("D", GameLogic.MDC.CharacterClass.Knight);

                    System.Threading.Thread.Sleep(3500);

                    while (true)
                    {
                        Console.WriteLine("Which client to end turn?");
                        var integer = int.Parse(Console.ReadLine());

                        if (client01.CurrentStatus == ClientProgram.Status.Busy)
                        {
                            client01.EndTurn();
                        }
                        if (client02.CurrentStatus == ClientProgram.Status.Busy)
                        {
                            client02.EndTurn();
                        }
                        System.Threading.Thread.Sleep(3000);
                        //string[] pos = Console.ReadLine().Split(new char[]{' '});
                        //client01.MovePlayer(int.Parse(pos[0] + ""), int.Parse(pos[1] + ""));
                        //client02.MovePlayer(int.Parse(pos[0] + ""), int.Parse(pos[1] + ""));
                    }
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
