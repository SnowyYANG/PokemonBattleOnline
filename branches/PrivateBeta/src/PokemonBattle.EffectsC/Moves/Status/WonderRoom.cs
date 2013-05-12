using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Status
{
  class WonderRoom : StatusMoveE
  {
    public WonderRoom(int id)
      : base(id)
    {
    }
    protected override void Act(AtkContext atk)
    {
      var c = atk.Controller;
      foreach (var pm in c.OnboardPokemons)
      {
        var stats = pm.OnboardPokemon.FiveD;
        var d = stats.Def;
        stats.Def = stats.SpDef;
        stats.SpDef = d;
      }
      if (c.Board.AddCondition("WonderRoom", c.TurnNumber + 4)) c.ReportBuilder.Add("EnWonderRoom");
      else
      {
        c.Board.RemoveCondition("WonderRoom");
        c.ReportBuilder.Add("DeWonderRoom");
      }
    }
  }
}
