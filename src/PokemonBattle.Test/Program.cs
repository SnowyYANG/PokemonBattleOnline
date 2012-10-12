using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.Tactic.Messaging;
using LightStudio.Tactic.Globalization;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Test
{
  class Program
  {
    static Program()
    {
      GameDataService.Load("Data");
      DataService.Load(new StringService() { Language = "Chinese" });
      DataService.String.DefaultLanguage = "Chinese";
      DataService.DataString.DefaultLanguage = "Chinese";
      Game.Host.Effects.EffectsRegister.Register();
      Tactic.Scripting.ExecuteAll("..\\src\\PokemonBattle.EffectsP");
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
