using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Network;
using PokemonBattleOnline.Network.Commands;
using System.Text.Json;
using System.Runtime.Serialization.Json;
using PokemonBattleOnline.Game.GameEvents;

namespace PokemonBattleOnline.Test
{
    class Program
    {
        static PboServer Server;
        
        static TestClient C00;
        static TestClient C01;
        static TestClient C10;
        static TestClient C11;

        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += (sender, e) => EndLog();
            GameString.Load("..\\..\\res", "zh", "en");
            Server = new PboServer(9999);
            Thread.Sleep(500);
            RoomController.GameStop += (r, u) => LogLine(r.ToString() + (u == null ? " " : " " + u.Name));
            C00 = new TestClient("P00", Seat.Player00);
            C01 = new TestClient("P01", Seat.Player01);
            C10 = new TestClient("P10", Seat.Player10);
            C11 = new TestClient("P11", Seat.Player11);
            Console.ReadKey();

        TEAM:
            C00.EditTeam(null);
            C01.EditTeam(C00.Team);
            C10.EditTeam(C01.Team);
            C11.EditTeam(C10.Team);

            LogLine("============BATTLE============");
        BATTLE:
            Thread.Sleep(500);
            if (C00.Battle() && C01.Battle() && C10.Battle() && C11.Battle()) goto BATTLE;
            else
            {
                Console.WriteLine("------------------------------");
                EndLog();
                goto TEAM;
            }
        }
        static StreamWriter log;
        static void BeginLog()
        {
            if (!Directory.Exists("logs")) Directory.CreateDirectory("logs");
            log = new StreamWriter("logs\\" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".txt", true, Encoding.Unicode);
        }
        public static void LogLine()
        {
            Console.WriteLine();
            if (log != null) log.WriteLine();
        }
        public static void LogLine(string text)
        {
            Console.WriteLine(text);
            if (log != null) log.WriteLine(text);
        }
        public static void Log(string text)
        {
            Console.Write(text);
            if (log != null) log.Write(text);
        }
        static void EndLog()
        {
            if (log != null)
            {
                log.Dispose();
                log = null;
            }
        }
    }
}
