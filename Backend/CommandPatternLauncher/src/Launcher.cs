using System;
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
        Console.WriteLine("MDC Launcher 0.01\nChoose program to launch\n[1] Client\n[2] Server");
        Console.Write("Number: ");
        int decision = int.Parse(Console.ReadLine());

        switch (decision)
        {
            case 1:
                Console.WriteLine("---------------------CLIENT---------------------");
                ClientProgram.StartClient();
                break;
            case 2:
                Console.WriteLine("---------------------SERVER---------------------");
                MasterServer.StartServer();
                break;
            default:
                break;
        }
    }
}