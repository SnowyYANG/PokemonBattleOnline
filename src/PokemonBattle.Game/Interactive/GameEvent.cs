using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.Host;

namespace LightStudio.PokemonBattle.Game
{
  [DataContract(Namespace = Namespaces.DEFAULT)]
  public abstract class GameEvent
  {
    protected GameOutward Game
    { get; private set; }

    protected IText GetGameLog(string key)
    {
      return GameService.Logs[key].Clone(Game);
    }
    public abstract IText GetGameLog();
    public virtual void Update(GameOutward game)
    {
      Game = game;
    }
    public virtual void Update(SimGame game)
    {
    }
  }
}
