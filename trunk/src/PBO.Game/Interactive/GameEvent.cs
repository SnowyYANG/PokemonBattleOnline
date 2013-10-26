using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Game
{
  [DataContract(Namespace = PBOMarks.JSON)]
  public abstract class GameEvent
  {
    protected GameOutward Game
    { get; private set; }

    protected LogText GetGameLog(string key)
    {
      var t = GameLogs.Log(key ?? "nokey");
      if (t == null)
      {
        t = GameLogs.Log("notfound").Clone(Game);
        t.SetData(key);
        return t;
      }
      return t.Clone(Game);
    }
    protected void AppendGameLog(string key, object arg0 = null, object arg1 = null, object arg2 = null, object arg3 = null)
    {
      var text = GetGameLog(key);
      text.SetData(arg0, arg1, arg2, arg3);
      Game.AppendGameLog(text);
    }
    protected void AppendGameLog(string key, params int[] data)
    {
      var text = GetGameLog(key);
      text.SetData(data.ValueOrDefault(0), data.ValueOrDefault(1), data.ValueOrDefault(2), data.ValueOrDefault(3));
      Game.AppendGameLog(text);
    }
    protected PokemonOutward GetPokemon(int id)
    {
      return Game.GetPokemon(id);
    }
    protected SimPokemon GetPokemon(SimGame game, int id)
    {
      return game.Pokemons.ValueOrDefault(id);
    }
    protected SimOnboardPokemon GetOnboardPokemon(SimGame game, int id)
    {
      return game.OnboardPokemons.FirstOrDefault((p) => p != null && p.Id == id);
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
