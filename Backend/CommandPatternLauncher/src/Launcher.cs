using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
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
        ClientProgram myClient = new ClientProgram();
        // MasterServer myServer = new MasterServer();

        MasterServer.StartServer();
        // myClient.StartClient();

    }
}