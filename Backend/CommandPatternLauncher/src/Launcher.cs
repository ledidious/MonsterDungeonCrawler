using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
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
        // MasterServer myServer = new MasterServer();

        Thread serverThread = new Thread(new ThreadStart(() => ServerInteraction()));
        serverThread.Start();

        myFirstClient.ConnectToServer();
        myFirstClient.CreateNewGame();

        ClientProgram mySecondClient = new ClientProgram();
        mySecondClient.ConnectToServer();
        mySecondClient.ConnectToGame(myFirstClient.GameSession_ID);

        ClientProgram myThirdClient = new ClientProgram();
        myThirdClient.ConnectToServer();
        myThirdClient.ConnectToGame(myFirstClient.GameSession_ID);

        ClientProgram myFourthClient = new ClientProgram();
        myFourthClient.ConnectToServer();
        myFourthClient.ConnectToGame(myFirstClient.GameSession_ID);

        //myFirstClient.CreateNewPlayerForSession("Snoop Dog", MDC.CharacterClasses.MeleeFighter);
        //mySecondClient.CreateNewPlayerForSession("Tux", MDC.CharacterClasses.RangeFighter);
        //myThirdClient.CreateNewPlayerForSession("Franz", MDC.CharacterClasses.MeleeFighter);

        // ClientProgram myFifthClient = new ClientProgram();
        // myFifthClient.ConnectToServer();
        // myFifthClient.ConnectToGame(myFirstClient.GameSession_ID);
    }

    /// <summary>
    /// Thread implementation for clients. Waits for commands from client. 
    /// </summary>
    private static void ServerInteraction()
    {
        MasterServer.StartServer();
    }
}