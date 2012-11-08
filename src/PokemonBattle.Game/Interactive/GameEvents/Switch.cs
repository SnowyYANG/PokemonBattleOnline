using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.Tactic.Messaging.Lobby;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.Host;

namespace LightStudio.PokemonBattle.Game.GameEvents
{
  [DataContract(Namespace = Namespaces.PBO)]
  internal class SendOut : GameEvent
  {
    [DataMember(EmitDefaultValue = false)]
    string Log;
    [DataMember]
    PokemonOutward[] Pms;

    internal SendOut(string log = null, params PokemonProxy[] pms)
    {
      Log = log;
      Pms = new PokemonOutward[pms.Length];
      for (int i = 0; i < pms.Length; ++i) Pms[i] = pms[i].GetOutward();
    }

    protected override void Update()
    {
      foreach (PokemonOutward p in Pms)
      {
        p.Init(Game);
        Game.Board[p.Position.Team, p.Position.X] = p;
        Game.Board.PokemonSentout(Game, p.Position.Team, p.Position.X);
      }
      AppendGameLog(Log ?? "SendOut" + Pms.Length, Pms.Select((p) => p.Id).ToArray());
      foreach (PokemonOutward p in Pms)
      {
        //if (p.Cha
      }
    }
    public override void Update(SimGame game)
    {
      if (game.Team.Pokemons.ContainsKey(Pms[0].Id))
        foreach (PokemonOutward p in Pms)
        {
          game.OnboardPokemons[p.Position.X] = new SimPokemon(game.Team.Pokemons[p.Id], p);
          if (p.Owner.Id == game.Player.Id) game.Player.SwitchPokemon(game.Settings.Mode.GetPokemonIndex(p.Position.X), game.Player.GetPokemonIndex(p.Id));
        }
    }
  }

  [DataContract(Namespace = Namespaces.PBO)]
  internal class Withdraw : GameEvent
  {
    [DataMember(EmitDefaultValue = false)]
    int Pm;

    [DataMember(EmitDefaultValue = false)]
    int Ab;

    public Withdraw(PokemonProxy pm)
    {
      Pm = pm.Id;
      Ab = pm.Ability.Id;
      if (Ab != Host.Sp.Abilities.REGENERATOR && Ab != Host.Sp.Abilities.NATURAL_CURE) Ab = 0;
    }

    int team, x;
    protected override void Update()
    {
      var pm = GetPokemon(Pm);
      team = pm.Position.Team;
      x = pm.Position.X;
      if (pm.Hp.Value == 0) pm.Faint();
      else
      {
        pm.Withdraw();
        if (Ab == Host.Sp.Abilities.NATURAL_CURE) pm.State = PokemonState.Normal;//for TeamOutward
      }
      Game.Board[team, x] = null;
    }
    public override void Update(SimGame game)
    {
      if (team == game.Player.TeamId)
      {
        var pm = game.OnboardPokemons[x].Pokemon;
        game.OnboardPokemons[x] = null;
        if (pm.Hp.Value == 0) pm.State = PokemonState.Faint;
        else
        {
          if (Ab == Host.Sp.Abilities.REGENERATOR) pm.SetHp(pm.Hp.Value + pm.Hp.Origin / 3);
          else if (Ab == Host.Sp.Abilities.NATURAL_CURE) pm.State = PokemonState.Normal;
        }
      }
    }
  }
}
