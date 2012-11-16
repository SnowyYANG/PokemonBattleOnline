using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Abilities
{
  class Imposter : AbilityE
  {
    public Imposter(int id)
      : base(id)
    {
    }
    public override void Attach(PokemonProxy pm)
    {
      var t = pm.Controller.Board[1 - pm.Pokemon.TeamId][pm.OnboardPokemon.X].Pokemon;
      if (pm.CanTransform(t))
      {
        Raise(pm);
        pm.Transform(t);
      }
    }
  }
}
