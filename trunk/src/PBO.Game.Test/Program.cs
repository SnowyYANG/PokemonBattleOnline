using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Network;

namespace PokemonBattleOnline.Test
{
  class Program
  {
    static ClientController C1;
    static ClientController C2;
    static SimPokemon[] PM1;
    static SimPokemon[] PM2;
    static InputRequest IR1;
    static InputRequest IR2;
    static Random Random = new Random(0);

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
      RoomController.GameStop += (r, u) => LogLine(r.ToString() + (u == null ? " " : " " + u.Name));
      LoginClient.LoginSucceed += (c) =>
      {
        if (C1 == null)
        {
          C1 = c.Controller;
          C1.Room.PropertyChanged += (sender, e) =>
            {
              if (e.PropertyName == "Game") C1.Room.Game.LogAppended += (t, s) =>
                {
                  if (s.HasFlag(LogStyle.NoBr)) Log(t);
                  else LogLine(t);
                };
              else if (e.PropertyName == "PlayerController" && C1.Room.PlayerController != null)
              {
                PM1 = C1.Room.PlayerController.Player.Pokemons.ToArray();
                C1.Room.PlayerController.RequireInput += (ir) => IR1 = ir;
              }
            };
          C1.NewRoom(null, new Network.GameSettings(GameMode.Single), Seat.Player00);
        }
        else
        {
          C2 = c.Controller;
          C2.Room.PropertyChanged += (sender, e) =>
            {
              if (e.PropertyName == "PlayerController" && C2.Room.PlayerController != null)
              {
                PM2 = C2.Room.PlayerController.Player.Pokemons.ToArray();
                C2.Room.PlayerController.RequireInput += (ir) => IR2 = ir;
              }
            };
          C2.EnterRoom(C2.Rooms.Last(), Seat.Player10);
        }
        Console.WriteLine(c.Controller.User.Name + "logined.");
      };
      var client1 = new LoginClient("127.0.0.1", 9999, "P1", 1);
      client1.BeginLogin();
      Thread.Sleep(1000);
      var client2 = new LoginClient("127.0.0.1", 9999, "P2", 1);
      client2.BeginLogin();

      var team1 = new List<PokemonData>();
      var team2 = new List<PokemonData>();

    TEAM:
      Thread.Sleep(2000);
      Team(team1, "TEAM1: ");
      Console.WriteLine();
      Console.WriteLine("============TEAM 1============");
      team2.Clear();
      foreach (var p in team1)
      {
        Console.WriteLine(UserData.Export(p));
        team2.Add(p.Clone());
      }
      Team(team2, "TEAM2: ");
      Console.WriteLine();
      BeginLog();
      LogLine("============TEAM 1============");
      foreach (var t in team1) LogLine(UserData.Export(t));
      LogLine("============TEAM 2============");
      foreach (var t in team2) LogLine(UserData.Export(t));
      LogLine("============BATTLE============");
      C1.Room.GamePrepare(team1.ToArray());
      C2.Room.GamePrepare(team2.ToArray());

    BATTLE:
      var ir1 = IR1;
      var ir2 = IR2;
      IR1 = IR2 = null;
      Thread.Sleep(500);
      if (Battle(C1.Room.PlayerController, "BATTLE1: ", ir1, PM1) && Battle(C2.Room.PlayerController, "BATTLE2: ", ir2, PM2)) goto BATTLE;
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
    static void LogLine(string text)
    {
      Console.WriteLine(text);
      if (log != null) log.WriteLine(text);
    }
    static void Log(string text)
    {
      Console.Write(text);
      if (log != null) log.Write(text);
    }
    static void EndLog()
    {
      log.Dispose();
      log = null;
    }
    private static void Team(List<PokemonData> team, string pre)
    {
    LOOP:
      Console.Write(pre);
      var line = Console.ReadLine();
      switch(line)
      {
        case "ok":
          return;
        case "preview":
          foreach (var t in team) Console.WriteLine(UserData.Export(t));
          break;
        case "remove":
          if (team.Count > 0) team.RemoveAt(team.Count - 1);
          break;
        case "clear":
          team.Clear();
          break;
        default:
          AutoTeam(team, line.Trim());
          break;
      }
      goto LOOP;
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
    public static bool Battle(PlayerController pc, string pre, InputRequest ir, SimPokemon[] pms)
    {
      if (ir != null)
      {
      LOOP:
        Console.Write(pre);
        var line = Console.ReadLine();
        var ai = new ActionInput(1);
        switch (line)
        {
          case "":
            if (pc.Game.OnboardPokemons[0] != null)
            {
              var moves = pc.Game.OnboardPokemons[0].Moves;
              int i;
              for (i = 0; i < 4; ++i) if (moves[i] == null) break;
              ai.UseMove(0, moves[Random.Next(0, i)], false);
            }
            else
            {
              var ii = new List<int>();
              for (int i = 1; i < 6; ++i)
              {
                var p = pc.Game.Player.GetPokemon(i);
                if (p != null && p.Hp.Value > 0) ii.Add(i);
              }
              ai.SendOut(0, pc.Game.Player.GetPokemon(ii[Random.Next(0, ii.Count)]));
            }
            break;
          case "ok":
            pc.GiveUp();
            return false;
          case "mega !1":
          case "mega !2":
          case "mega !3":
          case "mega !4":
            {
              var move = pc.Game.OnboardPokemons[0].Moves[line[6] - '1'];
              if (move == null) goto default;
              ai.UseMove(0, move, true);
            }
            break;
          case "!1":
          case "!2":
          case "!3":
          case "!4":
            {
              var move = pc.Game.OnboardPokemons[0].Moves[line[6] - '1'];
              if (move == null) goto default;
              ai.UseMove(0, move, false);
            }
            break;
          case "#1":
          case "#2":
          case "#3":
          case "#4":
          case "#5":
          case "#6":
            {
              var p = pms[line[1] - '1'];
              if (p == null || p.Hp.Value == 0 || p.Owner.GetPokemon(0) == p) goto default;
              if (pc.Game.OnboardPokemons[0] != null) ai.Switch(0, p);
              else ai.SendOut(0, p);
            }
            break;
          default:
            Console.WriteLine("ERROR");
            goto LOOP;
        }
        pc.Input(ai);
      }
      return true;
    }
  }
}
