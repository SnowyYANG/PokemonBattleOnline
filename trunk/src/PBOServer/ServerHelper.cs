using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Network;

namespace PokemonBattleOnline.PBO.Server
{
    static class ServerHelper
    {
        internal static void process(string command)
        {
            if (command.StartsWith("ban"))
            {
                command = command.Substring(3);
                char[] cd = { '.' };
                string[] st = command.Split(cd);
                byte[] ips = new byte[4];
                for (int i = 0; i < 4; i++)
                    ips[i] = (byte)Convert.ToInt32(st[i]);
                IPAddress ip = new IPAddress(ips);
                if (PBOServer.Current.Banlist.IndexOf(ip) >= 0)
                    Console.WriteLine(command + " is already in banlist.");
                else
                {
                    PBOServer.Current.Banlist.Add(ip);
                    Console.WriteLine(command + " banned.");
                }
            }
            else if (command.StartsWith("unban"))
            {
                command = command.Substring(5);
                char[] cd = { '.' };
                string[] st = command.Split(cd);
                byte[] ips = new byte[4];
                for (int i = 0; i < 4; i++)
                    ips[i] = (byte)Convert.ToInt32(st[i]);

                IPAddress ip = new IPAddress(ips);
                if (PBOServer.Current.Banlist.IndexOf(ip) >= 0)
                {
                    PBOServer.Current.Banlist.Remove(ip);
                    Console.WriteLine(command + " unbanned.");
                }
                else
                    Console.WriteLine(command + " isn't in banlist.");
            }
            else if (command == "list" || command == "l")
                PBOServer.Current.ListUsers();

        }
    }
}
