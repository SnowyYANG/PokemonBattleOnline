using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using PokemonBattleOnline.Game.Host;

namespace PokemonBattleOnline.Game.GameEvents
{
  [DataContract(Namespace = PBOMarks.JSON)]
  public class Substitute : GameEvent
  {
    public static Substitute EnSubstitute(PokemonProxy pm)
    {
      return new Substitute() { Pm = pm.Id };
    }
    internal static Substitute DeSubstitute(PokemonProxy pm)
    {
      return new Substitute() { Pm = pm.Id, De = true };
    }

    [DataMember(EmitDefaultValue = false)]
    bool De;
    [DataMember]
    int Pm;

    protected override void Update()
    {
      var pm = Game.GetPokemon(Pm);
      pm.IsSubstitute = !De;
      if (De)
      {
        pm.HideSubstitute();
        AppendGameLog("DeSubstitute", Pm);
      }
      else
      {
        var hp = -(pm.Hp.Origin >> 2);
        pm.Hp.Value += hp;
        pm.ShowSubstitute();
        AppendGameLog("EnSubstitute", Pm);
        AppendGameLog("Hp", hp);
      }
    }
    public override void Update(SimGame game)
    {
      if (!De)
      {
        var pm = GetPokemon(game, Pm);
        if (pm != null) pm.SetHp(pm.Hp.Value - (pm.Hp.Origin >> 2));
      }
    }
  }
}
