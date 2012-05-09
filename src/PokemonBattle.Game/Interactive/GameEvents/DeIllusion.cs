using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Interactive.GameEvents
{
  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class DeIllusion : GameEvent
  {
    [DataMember]
    private PokemonOutward Pm;

    public DeIllusion(PokemonProxy pm)
    {
      Pm = pm.GetOutward();
    }

    public override void Update(Game.GameOutward game)
    {
      base.Update(game);
    }
    public override IText GetGameLog()
    {
      IText t = GetGameLog("DeIllusion");
      t.SetData(Pm);
      return t;
    }
  }
}
