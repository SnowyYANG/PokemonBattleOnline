using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Host
{
  [DataContract(Name = "eer", Namespace = Namespaces.PBO)]
  internal class RestGameEvent : GameEvent
  {
    [DataMember(Name = "a")]
    int Pm;

    public RestGameEvent(PokemonProxy pm)
    {
      Pm = pm.Id;
    }

    protected override void Update()
    {
      var pm = GetPokemon(Pm);
      pm.Hp.Value = pm.Hp.Origin;
      pm.State = PokemonState.SLP;
      AppendGameLog("Rest", Pm);
    }
    public override void Update(SimGame game)
    {
      var pm = GetPokemon(game, Pm);
      if (pm != null)
      {
        pm.SetHp(pm.Hp.Origin);
        pm.State = PokemonState.SLP;
      }
    }
  }
}
