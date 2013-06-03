using System;
using System.Collections.Generic;
using System.Text;
using NetworkLib.Utilities;
using System.Diagnostics;
namespace PokemonBattle.RoomListServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.SetTraceLogger();
            Logger.Level = LogLevel.Info;


            RoomListServer server = new RoomListServer();
            if (server.Initialize())
            {
                server.Run();
            }
            else
            {
                Console.WriteLine("Fail to start server");
            }
        }
    }
}
