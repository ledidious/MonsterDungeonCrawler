using System;
using System.Threading;
using MDC.Client;
using MDC.Server;

class Launcher
{
    /// <summary>
    /// The Launcher is a temporary solution to test the server/client implementation.
    /// </summary>
    /// <param name="args"></param>
    static void Main(string[] args)
    {
        ClientProgram myFirstClient = new ClientProgram();

        Thread serverThread = new Thread(new ThreadStart(() => MasterServer.StartServer()));
        serverThread.Start();

        System.Threading.Thread.Sleep(500);

        myFirstClient.ConnectToServer();
        myFirstClient.CreateNewGame();
        // myFirstClient.DisconnectFromServer();
/* 
        System.Threading.Thread.Sleep(500);

        ClientProgram mySecondClient = new ClientProgram();
        mySecondClient.ConnectToServer();
        mySecondClient.ConnectToGame(myFirstClient.GameSession_ID);
        // try
        // {
        //     mySecondClient.ConnectToGame("4711");
        // }
        // catch (System.Exception)
        // {

        //     throw;
        // }

        // //############################################
        // ClientProgram elChappo01 = new ClientProgram();
        // elChappo01.ConnectToServer();
        // elChappo01.CreateNewGame();
        // System.Threading.Thread.Sleep(500);
        // ClientProgram elChappo02 = new ClientProgram();
        // elChappo02.ConnectToServer();
        // //############################################

        ClientProgram myThirdClient = new ClientProgram();
        myThirdClient.ConnectToServer();

        // elChappo02.ConnectToGame(elChappo01.GameSession_ID);
        myThirdClient.ConnectToGame(myFirstClient.GameSession_ID);

        ClientProgram myFourthClient = new ClientProgram();
        myFourthClient.ConnectToServer();
        myFourthClient.ConnectToGame(myFirstClient.GameSession_ID); */


        // ClientProgram myFifthClient = new ClientProgram();
        // myFifthClient.ConnectToServer();
        // myFifthClient.ConnectToGame(myFirstClient.GameSession_ID);

        /* 

        myFirstClient.CreateNewPlayerForSession("Snoop Dog", MDC.CharacterClass.MeleeFighter);
        mySecondClient.CreateNewPlayerForSession("Tux", MDC.CharacterClass.RangeFighter);
        myThirdClient.CreateNewPlayerForSession("Franz", MDC.CharacterClass.MeleeFighter);
        myFourthClient.CreateNewPlayerForSession("Pepe", MDC.CharacterClass.MeleeFighter);

        myFirstClient.StartCreatedGame();

        myFirstClient.MovePlayer(1, 0);
        myFirstClient.MovePlayer(2, 0);
        myFirstClient.MovePlayer(2, 1);
        // myFirstClient.EndTurn();
        myFirstClient.MovePlayer(2, 2);
        myFirstClient.MovePlayer(3, 2);

        System.Threading.Thread.Sleep(2000);
        mySecondClient.MovePlayer(18, 2);

*/
        // System.Threading.Thread.Sleep(5000);
        // myFirstClient.DisconnectFromServer();
        while (true)
        {

        }
        // ClientProgram myFifthClient = new ClientProgram();
        // myFifthClient.ConnectToServer();
        // myFifthClient.ConnectToGame(myFirstClient.GameSession_ID);
    }
}