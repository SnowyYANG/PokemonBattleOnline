using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Game.Host;

namespace LightStudio.PokemonBattle.Game.GameEvents
{
  [DataContract(Namespace = Namespaces.DEFAULT)]
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

    public override void Update(GameOutward game)
    {
      base.Update(game);
      var pm = game.GetPokemon(Pm);
      if (De) pm.HideSubstitute();
      else pm.ShowSubstitute();
    }
    public override IText GetGameLog()
    {
      IText t = De? GetGameLog("DeSubstitute") : GetGameLog("EnSubstitute");
      t.SetData(Pm);
      return t;
    }
  }
}
