using System;
using GameLogic.MDC.Client;
using GameLogic.MDC.Server;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("[1] Server\n[2] Simulate Client\n[3] Demo Mode");
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
                    Console.WriteLine("How many clients?");
                    int clientCount = int.Parse(Console.ReadLine());

                    var clients = new ClientProgram[clientCount];
                    for (int i = 0; i < clientCount; i++)
                    {
                        clients[i] = new ClientProgram();
                    }

                    Console.WriteLine("Please enter session id");
                    string sessionID = Console.ReadLine();

                    foreach (var client in clients)
                    {
                        client.ConnectToServer();
                        client.ConnectToGame(sessionID);
                        client.CreateNewPlayerForSession((int) (new Random().Next() * 100) + "", GameLogic.MDC.CharacterClass.Knight);

                    }

                    System.Threading.Thread.Sleep(3500);

                    while (true)
                    {
                        foreach (var client in clients)
                        {
                            if (client.CurrentStatus == ClientProgram.Status.Busy)
                            {
                                client.EndTurn();
                            }
                        }

                        System.Threading.Thread.Sleep(1000);
                    }
                default:
                    Console.WriteLine("Input not recognized");
                    break;
            }

            while (true)
            {

            }
        }
    }
}
