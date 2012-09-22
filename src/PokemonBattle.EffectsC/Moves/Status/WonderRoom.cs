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
    private void Exchange(SixD stats)
    {
      var d = stats.Def;
      stats.Def = stats.SpDef;
      stats.SpDef = d;
    }
    protected override void Act(AtkContext atk)
    {
      foreach (var pm in atk.Controller.OnboardPokemons)
        Exchange(pm.OnboardPokemon.Static);
      if (atk.Controller.Board.AddCondition("WonderRoom")) atk.Controller.ReportBuilder.Add("EnWonderRoom");
      else atk.Controller.ReportBuilder.Add("DeWonderRoom");
    }
  }
}
