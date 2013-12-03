using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Test
{
  class Program
  {
    static void Main(string[] args)
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
