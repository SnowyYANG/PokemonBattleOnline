﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.Tactic.DataModels;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Interactive.GameEvents
{
  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class SendOut : GameEvent
  {
    [DataMember]
    public int PlayerId { get; private set; }
    [DataMember]
    public PokemonOutward Pokemon { get; private set; }

    internal SendOut(int playerId, PokemonOutward pokemon)
    {
      PlayerId = playerId;
      Pokemon = pokemon;
    }
    public override IText GetGameLog()
    {
      IText t = GetGameLog(SENDOUT);
      t.SetData(this);
      return t;
    }
    public override void Update(GameOutward game)
    {
      game.Board[Pokemon.Position.Team, Pokemon.Position.X] = Pokemon;
      game.Board.PokemonSentout(Pokemon.Position.Team, Pokemon.Position.X);
    }
    public override void Update(SimGame game)
    {
      if (Pokemon.Position.Team == game.Team.Id)
      {
        game.Pokemons[Pokemon.Position.X] = new SimPokemon(game.Team.Pokemons[Pokemon.Id], Pokemon);
        game.ActivePokemons.Add(Pokemon.Position.X, game.Pokemons[Pokemon.Position.X]);
        if (Pokemon.OwnerId == game.Player.Id)
        {
          game.Player.SwitchPokemon(game.Settings.Mode.GetPokemonIndex(Pokemon.Position.X), game.Player.GetPokemonIndex(Pokemon.Id));
        }
      }
    }
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class Withdraw : GameEvent
  {
    [DataMember]
    int Team { get; set; }

    [DataMember]
    int X { get; set; }

    internal Withdraw(PokemonProxy pm)
    {
      Team = pm.Tile.Team;
      X = pm.Tile.X;
    }
    public override IText GetGameLog()
    {
      System.Diagnostics.Debugger.Break();
      return null;
    }
    public override void Update(GameOutward game)
    {
      PokemonOutward pm = game.Board[Team, X];
      if (pm.Hp.Value == 0)
      {
        pm.Faint();
        //text = xxx倒下了
      }
      else
      {
        pm.Withdrawn();
        //text = xxx把xxx收了回去
        //else ;//xxx回到了xxx身边
      }
      game.Board[Team, X] = null;
    }
    public override void Update(SimGame game)
    {
      if (Team == game.Player.TeamId)
      {
        game.ActivePokemons.Remove(X);
        game.Pokemons[X] = null;
      }
    }
  }
}