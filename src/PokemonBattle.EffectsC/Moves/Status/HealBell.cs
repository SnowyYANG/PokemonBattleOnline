using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Status
{
  [DataContract(Namespace = Namespaces.PBO)]
  class HealBellEvent : GameEvent
  {
    [DataMember(EmitDefaultValue = false)]
    int Pm;

    [DataMember(EmitDefaultValue = false)]
    string Key;

    public HealBellEvent(PokemonProxy pm, string key)
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
      if (game.Team.Id == team)
      {
        var player = GetPokemon(game, Pm).Owner;
        foreach (var pm in game.OnboardPokemons)
          if (pm != null) pm.Pokemon.ClientChangePokemonState(PokemonState.Normal);
        foreach (var pm in player.Pokemons)
          if (pm.State != PokemonState.Faint) pm.ClientChangePokemonState(PokemonState.Normal);
      }
    }
  }
  class HealBell : StatusMoveE
  {
    static HealBell()
    {
      EffectsService.Register<HealBellEvent>();
    }

    private readonly string log;

    public HealBell(int id, string log)
      : base(id)
    {
      this.log = log;
    }

    protected override void Act(AtkContext atk)
    {
      var aer = atk.Attacker;
      aer.Controller.ReportBuilder.Add(new HealBellEvent(aer, log));
      foreach (var pm in aer.Tile.Field.Pokemons)
        if (pm.State != PokemonState.Normal) pm.Pokemon.State = PokemonState.Normal;
      foreach(var pm in aer.Pokemon.Owner.Pokemons)
        if (pm.Hp.Value > 0 && pm.State != PokemonState.Normal) pm.State = PokemonState.Normal;
    }
  }
}
