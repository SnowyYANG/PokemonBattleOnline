﻿using System;
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
    int Player;
    [DataMember]
    PokemonOutward[] Pms;

    internal SendOut(string log = null, params PokemonProxy[] pms)
    {
      Log = log;
      Player = pms[0].Pokemon.Owner.Id;
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
      var args = new List<int>();
      args.Add(Player);
      args.AddRange(Pms.Select((p) => p.Id));
      AppendGameLog(Log ?? "SendOut" + Pms.Length, args.ToArray());
    }
    public override void Update(SimGame game)
    {
      if (game.Team.GetPlayerIndex(Player) != -1)
        foreach (PokemonOutward p in Pms)
        {
          game.OnboardPokemons[p.Position.X] = new SimPokemon(game.Team.Pokemons[p.Id], p);
          if (Player == game.Player.Id) game.Player.SwitchPokemon(game.Settings.Mode.GetPokemonIndex(p.Position.X), game.Player.GetPokemonIndex(p.Id));
        }
    }
  }

  [DataContract(Namespace = Namespaces.PBO)]
  internal class Withdraw : GameEvent
  {
    [DataMember(EmitDefaultValue = false)]
    string Log;
    
    [DataMember(EmitDefaultValue = false)]
    int Pm;

    [DataMember(EmitDefaultValue = false)]
    int Ab;

    public Withdraw(PokemonProxy pm, string log = null)
    {
      Log = log == null ? pm.Hp == 0 ? "Faint" : null : log;
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
      if (Log == "Faint") pm.Hp.Value = 0;
      if (pm.Hp.Value == 0)
      {
        pm.Faint();
        AppendGameLog("Faint", pm.Id);
      }
      else
      {
        pm.Withdraw();
        if (Ab == Host.Sp.Abilities.NATURAL_CURE) pm.State = PokemonState.Normal;//for TeamOutward
        if (Log != null) AppendGameLog(Log ?? "Withdraw", pm.OwnerId, pm.Id);
      }
      Game.Board[team, x] = null;
    }
    public override void Update(SimGame game)
    {
      if (team == game.Player.TeamId)
      {
        var pm = game.OnboardPokemons[x].Pokemon;
        game.OnboardPokemons[x] = null;
        if (Log == "Faint") pm.SetHp(0);
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
