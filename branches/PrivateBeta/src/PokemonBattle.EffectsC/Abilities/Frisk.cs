using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.Host;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Abilities
{
  class Frisk : AbilityE
  {
    public Frisk(int id)
      : base(id)
    {
    }
    public override void Attach(PokemonProxy pm)
    {
      var pms = pm.Controller.GetOnboardPokemons(1 - pm.Pokemon.TeamId);
      var items = new List<Item>();
      foreach (var p in pms)
        if (p.Pokemon.Item != null) items.Add(p.Pokemon.Item);
      if (items.Count == 0) return;
      int i = pm.Controller.GetRandomInt(0, items.Count - 1);
      Raise(pm);
      pm.Controller.ReportBuilder.Add("Frisk", pm, items[i]);
    }
  }
}
