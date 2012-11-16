using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.Host;

namespace LightStudio.PokemonBattle.Game
{
  [DataContract(Namespace = Namespaces.PBO)]
  public abstract class GameEvent
  {
    private int? _sleep;
    public int Sleep
    {
      get { return _sleep ?? 100; }
      protected set { _sleep = value; }
    }
    protected GameOutward Game
    { get; private set; }

    protected IText GetGameLog(string key)
    {
      var t = GameService.Logs[key ?? "nokey"];
      if (t == null)
      {
        t = GameService.Logs["notfound"].Clone(Game);
        t.SetData(key);
        return t;
      }
      return t.Clone(Game);
    }
    protected void AppendGameLog(string key, object arg0 = null, object arg1 = null, object arg2 = null, object arg3 = null)
    {
      IText text = GetGameLog(key);
      text.SetData(arg0, arg1, arg2, arg3);
      Game.AppendGameLog(text);
    }
    protected void AppendGameLog(string key, params int[] data)
    {
      IText text = GetGameLog(key);
      text.SetData(data.ValueOrDefault(0), data.ValueOrDefault(1), data.ValueOrDefault(2), data.ValueOrDefault(3));
      Game.AppendGameLog(text);
    }
    protected PokemonOutward GetPokemon(int id)
    {
      return Game.GetPokemon(id);
    }
    protected Pokemon GetPokemon(SimGame game, int id)
    {
      return game.Team.Pokemons.ValueOrDefault(id);
    }
    protected virtual void Update()
    {
    }
    public void Update(GameOutward game)
    {
      Game = game;
      Update();
    }
    public virtual void Update(SimGame game)
    {
    }
  }
}
