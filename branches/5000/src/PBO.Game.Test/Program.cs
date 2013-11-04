using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Tactic.Globalization;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Tactic;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Test
{
  class Program
  {
    static Program()
    {
      //ReportFragment rf = new ReportFragment(0, null, null, Weather.Normal);
      //rf.AddEvent(new Game.GameEvents.SimpleEvent("test", 1, "Atk"));
      //var rf = Tactic.Network.Commands.UserChanged<int>.AddUser(1, "a", 3);

      //var buffer = Serializer.SerializeToJson(rf);
      //Console.WriteLine(Encoding.UTF8.GetString(buffer));
      //var o = Serializer.DeserializeFromJson<Tactic.Network.Commands.UserChanged<int>>(buffer);
      //Console.WriteLine(o);
      
      //GameDataService.Load("Data");
      //DataService.Load(new StringService() { Language = "Chinese" });
      //DataService.String.DefaultLanguage = "Chinese";
      //DataService.DataString.DefaultLanguage = "Chinese";
    }
    
    static void Main(string[] args)
    {
      using (var ui = new TestUIDispatcher())
      {
        UIDispatcher.Init(ui);

        NewTest();

        Console.ReadKey();
        Console.ReadKey();
        Console.ReadKey();
      }
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
