using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Network;

namespace PokemonBattleOnline.Test
{
  class Program
  {
    static Client C1;
    static Client C2;

    static void Main(string[] args)
    {
      using (var pack = new ZipData("..\\res\\rom.zip"))
      {
        RomData.Load(pack, "/rom.xml");
        LearnList.Load(pack, "/learnset");
      }
      GameString.Load("..\\res\\string", "zh", "en");
      PBOServer.NewServer(9999);
      Thread.Sleep(1000);
      LoginClient.LoginSucceed += (c) =>
      {
        if (C1 == null) C1 = c;
        else C2 = c;
        Console.WriteLine(c.Controller.User.Name + "logined.");
      };
      var client1 = new LoginClient("127.0.0.1", 9999, "P1", 1);
      client1.BeginLogin();
      var client2 = new LoginClient("127.0.0.1", 9999, "P2", 1);
      client2.BeginLogin();

      var team1 = new List<PokemonData>();
      var team2 = new List<PokemonData>();
      string line;
      Thread.Sleep(2000);
      Console.WriteLine("Set TEAM1:");
    TEAM1:
      line = Console.ReadLine();
      if (line == "preview") foreach (var t in team1) Console.WriteLine(UserData.Export(t));
      if (line == "OK")
      {
        Console.WriteLine();
        Console.WriteLine("============TEAM1============");
        foreach (var t in team1) Console.WriteLine(UserData.Export(t));
        Console.WriteLine();
        Console.WriteLine("Set TEAM2:");
        goto TEAM2;
      }
      AutoTeam(team1, line.Trim());
      goto TEAM1;
    TEAM2:
      line = Console.ReadLine();
      if (line == "preview") foreach (var t in team2) Console.WriteLine(UserData.Export(t));
      if (line == "OK")
      {
        Console.WriteLine();
        Console.WriteLine("============TEAM1============");
        foreach (var t in team1) Console.WriteLine(UserData.Export(t));
        Console.WriteLine("============TEAM2============");
        foreach (var t in team2) Console.WriteLine(UserData.Export(t));
        Console.WriteLine();
        goto BATTLE;
      }
      AutoTeam(team2, line.Trim());
      goto TEAM2;
    BATTLE:
      var key = Console.ReadKey();
      if (key.Key == ConsoleKey.Escape) goto TEAM1;
      else ;
    }

    public static void AutoTeam(List<PokemonData> team, string text)
    {
      int end;
    LOOP:
      var r = GameString.Find(text, out end);
      if (end == 0)
      {
        if (!string.IsNullOrWhiteSpace(text)) Console.WriteLine(@"<ERROR> """ + text + @"""");
        return;
      }
      var n = r.Substring(1).ToInt();
      switch (r[0])
      {
        case 'p':
          team.Add(new PokemonData(n / 100, n % 100));
          break;
        case 'm':
          {
            var pm = team.LastOrDefault();
            if (pm == null)
            {
              pm = new PokemonData(235, 0);
              team.Add(pm);
            }
            pm.AddMove(RomData.GetMove(n));
          }
          break;
        case 'a':
          {
            var pm = team.LastOrDefault();
            if (pm == null || !pm.Form.Data.Abilities.Contains(n))
            {
              foreach(var p in RomData.Pokemons)
                if (p.Forms.First().Data.Abilities.Contains(n))
                {
                  pm = new PokemonData(p.Number, 0);
                  team.Add(pm);
                  break;
                }
            }
            pm.Ability = n;
          }
          break;
        case 'i':
          {
            var pm = team.LastOrDefault();
            if (pm == null || pm.Item != 0)
            {
              pm = new PokemonData(235, 0);
              team.Add(pm);
            }
            pm.Item = n;
          }
          break;
      }
      text = text.Substring(end);
      goto LOOP;
    }
    public static void Battle()
    {
    }
    public static void NewTest()
    {
      Host h = new Host();
      h.GameEnd += AIController.TestEnd;
      var p1 = h.AddPlayer(AIController.GetP1Team());
      var p2 = h.AddPlayer(AIController.GetP2Team());
      AIController.TestBegin(p1, p2);
      p1.RequireInput += new Action<Game.InputRequest>(AIController.P1_RequireInput);
      p2.RequireInput += new Action<Game.InputRequest>(AIController.P2_RequireInput);
      h.StartGame();
    }
  }
}
