using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.Host;

namespace LightStudio.PokemonBattle.Game.GameEvents
{
  [DataContract(Name = "eer", Namespace = PBOMarks.PBO)]
  internal class RestGame : GameEvent
  {
    [DataMember(Name = "a")]
    int Pm;

    public RestGame(PokemonProxy pm)
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
