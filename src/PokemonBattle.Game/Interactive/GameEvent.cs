using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Interactive
{
  [DataContract(Namespace = Namespaces.DEFAULT)]
  public abstract class GameEvent
  {
    protected static IText GetGameLog(string type)
    {
      return GameService.Logs[type].Clone();
    }

    public abstract IText GetGameLog();
    public virtual void Update(GameOutward game)
    {
    }
    public virtual void Update(SimGame game)
    {
    }
  }
}
