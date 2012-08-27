using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Game.Host;

namespace LightStudio.PokemonBattle.Game.GameEvents
{
  [DataContract(Namespace = Namespaces.LIGHT)]
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
      if (De) pm.HideSubstitute();
      else
      {
        pm.Hp.Value -= pm.Hp.Origin >> 2;
        pm.ShowSubstitute();
      }
      AppendGameLog(De ? "DeSubstitute" : "EnSubstitute", Pm);
    }
  }
}
