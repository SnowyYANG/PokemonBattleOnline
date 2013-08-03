using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Game.Host;

namespace LightStudio.PokemonBattle.Game.GameEvents
{
  [DataContract(Namespace = PBOMarks.PBO)]
  class HealBell : GameEvent
  {
    [DataMember(EmitDefaultValue = false)]
    int Pm;

    [DataMember(EmitDefaultValue = false)]
    string Key;

    public HealBell(PokemonProxy pm, string key)
    {
      Pm = pm.Id;
      Key = key == "HealBell" ? null : key;
    }

    private int team;
    protected override void Update()
    {
      AppendGameLog(Key ?? "HealBell");
      var pm = Game.GetPokemon(Pm);
      team = pm.Position.Team;
      for (int x = 0; x < Game.Settings.Mode.XBound(); ++x)
      {
        var p = Game.Board[team, x];
        if (p != null && p.State != PokemonState.Normal) p.State = PokemonState.Normal;
      }
      Game.Teams[team].HealBell();
    }
    public override void Update(SimGame game)
    {
      if (game.Player.Team == team)
      {
        var player = GetPokemon(game, Pm).Owner;
        foreach (var pm in game.OnboardPokemons)
          if (pm != null) pm.Pokemon.State = PokemonState.Normal;
        foreach (var pm in player.Pokemons)
          if (pm.State != PokemonState.Faint) pm.State = PokemonState.Normal;
      }
    }
  }
}
