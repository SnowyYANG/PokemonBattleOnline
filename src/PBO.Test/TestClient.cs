using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Network;

namespace PokemonBattleOnline.Test
{
    class TestClient
    {
        public string Name;
        public ClientController C;
        Seat Seat;
        public List<PokemonData> Team = new List<PokemonData>();
        public SimPokemon[] PM;
        public InputRequest IR;

        public TestClient(ClientController c, Seat seat)
        {
            Name = c.User.Name;
            C = c;
            Seat = seat;
            if (Seat == Seat.Player00)
                C.Room.PropertyChanged += (sender, e) =>
                {
                    if (e.PropertyName == "Game")
                    {
                        C.Room.Game.LogAppended += (t, s) =>
                        {
                            if (s.HasFlag(LogStyle.NoBr)) Program.Log(t);
                            else Program.LogLine(t);
                        };
                    }
                };
            C.Room.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "PlayerController" && C.Room.PlayerController != null)
                {
                    PM = C.Room.PlayerController.Player.Pokemons.ToArray();
                    C.Room.PlayerController.RequireInput += (ir) => IR = ir;
                }
            };
        }

        public void ShowTeam()
        {
            Console.WriteLine("====================");
            Console.WriteLine(Name);
            Console.WriteLine("--------------------");
            foreach (var t in Team) Console.WriteLine(UserData.Export(t));
            Console.WriteLine("====================");
        }
        public void EditTeam(List<PokemonData> team)
        {
            if (C.Room.Room == null) C.EnterRoom(C.Rooms.Last(), Seat);
            if (team != null)
            {
                Team.Clear();
                foreach (var p in team) Team.Add(p.Clone());
            }
            LOOP:
            Console.Write(Name + " Team: ");
            var line = Console.ReadLine();
            switch (line)
            {
                case "ok":
                    ShowTeam();
                    C.Room.GamePrepare(Team.ToArray());
                    return;
                case "preview":
                    ShowTeam();
                    break;
                case "remove":
                    if (Team.Count > 0) Team.RemoveAt(Team.Count - 1);
                    break;
                case "clear":
                    Team.Clear();
                    break;
                default:
                    AutoTeam(line.Trim());
                    break;
            }
            goto LOOP;
        }
        void AutoTeam(string text)
        {
            int end;
            LOOP:
            var r = GameString.Find(text, out end);
            if (end == 0)
            {
                if (!string.IsNullOrWhiteSpace(text)) Console.WriteLine(@"<ERROR> """ + text + @"""");
                return;
            }
            var n = int.Parse(r.Substring(1));
            switch (r[0])
            {
                case 'p':
                    Team.Add(new PokemonData(n / 100, n % 100));
                    break;
                case 'm':
                    {
                        var pm = Team.LastOrDefault();
                        if (pm == null)
                        {
                            pm = new PokemonData(235, 0);
                            Team.Add(pm);
                        }
                        pm.AddMove(RomData.GetMove(n));
                    }
                    break;
                case 'a':
                    {
                        var pm = Team.LastOrDefault();
                        if (pm == null || !pm.Form.Data.Abilities.Contains(n))
                        {
                            foreach (var p in RomData.Pokemons)
                                if (p.Forms.First().Data.Abilities.Contains(n))
                                {
                                    pm = new PokemonData(p.Number, 0);
                                    Team.Add(pm);
                                    break;
                                }
                        }
                        pm.Ability = n;
                    }
                    break;
                case 'i':
                    {
                        var pm = Team.LastOrDefault();
                        if (pm == null || pm.Item != 0)
                        {
                            pm = new PokemonData(235, 0);
                            Team.Add(pm);
                        }
                        pm.Item = n;
                    }
                    break;
            }
            text = text.Substring(end);
            goto LOOP;
        }

        static Random Random = new Random(0);
        public bool Battle()
        {
            var pc = C.Room.PlayerController;
            if (IR != null)
            {
                var ir = IR;
                IR = null;
                ir.Init(pc.Game);
                LOOP:
                Console.Write(Name + " Battle: ");
                var line = Console.ReadLine();
                var ai = new ActionInput(pc.Game.Settings.Mode.XBound());
                switch (line)
                {
                    case "":
                        if (pc.Game.OnboardPokemons[ir.CurrentX] != null)
                        {
                            var moves = pc.Game.OnboardPokemons[ir.CurrentX].Moves;
                            int i;
                            for (i = 0; i < 4; ++i) if (moves[i] == null) break;
                            ai.UseMove(ir.CurrentX, moves[Random.Next(0, i)], false);
                        }
                        else
                        {
                            var ii = new List<int>();
                            for (int i = 1; i < 6; ++i)
                            {
                                var p = pc.Game.Player.GetPokemon(i);
                                if (p != null && p.Hp.Value > 0) ii.Add(i);
                            }
                            ai.SendOut(ir.CurrentX, pc.Game.Player.GetPokemon(ii[Random.Next(0, ii.Count)]));
                        }
                        break;
                    case "ok":
                        pc.GiveUp();
                        return false;
                    case "mega !1":
                    case "mega !2":
                    case "mega !3":
                    case "mega !4":
                        if (ir.CanMega)
                        {
                            var move = pc.Game.OnboardPokemons[ir.CurrentX].Moves[line[6] - '1'];
                            if (move == null) goto default;
                            ai.UseMove(ir.CurrentX, move, true);
                        }
                        else Console.WriteLine("Cannot Mega");
                        break;
                    case "!1":
                    case "!2":
                    case "!3":
                    case "!4":
                        {
                            var move = pc.Game.OnboardPokemons[ir.CurrentX].Moves[line[1] - '1'];
                            if (move == null) goto default;
                            ai.UseMove(ir.CurrentX, move, false);
                        }
                        break;
                    case "#1":
                    case "#2":
                    case "#3":
                    case "#4":
                    case "#5":
                    case "#6":
                        {
                            var p = PM[line[1] - '1'];
                            if (p == null || p.Hp.Value == 0 || p.Owner.GetPokemon(0) == p) goto default;
                            if (pc.Game.OnboardPokemons[ir.CurrentX] != null) ai.Switch(ir.CurrentX, p);
                            else ai.SendOut(ir.CurrentX, p);
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
