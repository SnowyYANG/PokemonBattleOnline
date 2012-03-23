using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.Tactic.DataModels;
using LightStudio.Tactic.Messaging.Lobby;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Interactive.GameEvents
{
  [DataContract(Namespace = Namespaces.DEFAULT)]
  internal class SendOut : GameEvent
  {
    [DataMember]
    public int PlayerId { get; private set; }
    [DataMember]
    public PokemonOutward[] Pms { get; private set; }

    internal SendOut(params PokemonProxy[] pms)
    {
      PlayerId = pms[0].Pokemon.Id;
      Pms = new PokemonOutward[pms.Length];
      for (int i = 0; i < pms.Length; ++i) Pms[i] = pms[i].Outward;
    }

    public override IText GetGameLog()
    {
      IText t = GetGameLog(SENDOUT);
      //
      t.SetData(LobbyService.GetUserName(PlayerId),  DataService.GameLog.ConvertMultiObjects(
        (p) => string.Format("{0}(Lv.{1} {2})", p.Name, p.Lv, DataService.DataString[DataService.GetPokemonType(p.ImageId).Name]), Pms));
      return t;
    }
    public override void Update(GameOutward game)
    {
      foreach (PokemonOutward p in Pms)
      {
        game.Board[p.Position.Team, p.Position.X] = p;
        game.Board.PokemonSentout(p.Position.Team, p.Position.X);
      }
    }
    public override void Update(SimGame game)
    {
      if (game.Team.HasPlayer(PlayerId))
        foreach (PokemonOutward p in Pms)
        {
          game.Pokemons[p.Position.X] = new SimPokemon(game.Team.Pokemons[p.Id], p);
          game.ActivePokemons.Add(p.Position.X, game.Pokemons[p.Position.X]);
          if (PlayerId == game.Player.Id)
          {
            game.Player.SwitchPokemon(game.Settings.Mode.GetPokemonIndex(p.Position.X), game.Player.GetPokemonIndex(p.Id));
          }
        }
    }
  }

  [DataContract(Namespace = Namespaces.DEFAULT)]
  internal class Withdraw : GameEvent
  {
    [DataMember]
    int Team { get; set; }

    [DataMember]
    int X { get; set; }

    string pokemonName;
    string playerName;
    bool isFaint;

    public Withdraw(PokemonProxy pm)
    {
      Team = pm.Tile.Team;
      X = pm.Tile.X;
    }
    public override IText GetGameLog()
    {
      IText t;
      if (isFaint)
      {
        t = GetGameLog(FAINT);
        if (t != null) t.SetData(pokemonName);
      }
      else
      {
        t = GetGameLog(WITHDRAW);
        if (t != null) t.SetData(playerName, pokemonName);
      }
      return t;
    }
    public override void Update(GameOutward game)
    {
      PokemonOutward pm = game.Board[Team, X];
      pokemonName = pm.Name;
      playerName = LobbyService.GetUserName(pm.OwnerId);
      if (pm.Hp.Value == 0)
      {
        pm.Faint();
        isFaint = true;
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
