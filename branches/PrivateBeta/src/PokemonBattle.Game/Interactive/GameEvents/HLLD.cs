using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Game.Host;

namespace LightStudio.PokemonBattle.Game.GameEvents
{
  [DataContract(Namespace = PBOMarks.PBO)]
  class HLLD : GameEvent
  {
    [DataMember(EmitDefaultValue = false)]
    int Pm;
    [DataMember(EmitDefaultValue = false)]
    bool PP;

    public HLLD(PokemonProxy pm, bool resetPP)
    {
      Pm = pm.Id;
      PP = resetPP;
    }

    protected override void Update()
    {
      var pm = GetPokemon(Pm);
      pm.Hp.Value = pm.Hp.Origin;
      pm.State = PokemonState.Normal;
      if (PP) AppendGameLog("LunarDance", Pm);
      else AppendGameLog("HealingWish", Pm);
    }
    public override void Update(SimGame game)
    {
      var pm = GetPokemon(game, Pm);
      if (pm != null)
      {
        pm.SetHp(pm.Hp.Origin);
        pm.State = PokemonState.Normal;
        if (PP)
          foreach (var m in pm.Moves) ((PairValue)m.PP).Value = m.PP.Origin;
      }
    }
  }
}
