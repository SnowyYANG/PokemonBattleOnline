using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Interactive;

namespace LightStudio.PokemonBattle.Game
{
  /// <summary>
  /// 这必须是单线程的...
  /// </summary>
  internal class GameContext : IGame
  {
    public event Action<int, int> GameEnd;
    public event Action<ReportFragment> ReportUpdated
    {
      add { Controller.ReportUpdated += value; }
      remove { Controller.ReportUpdated -= value; }
    }
    public event Action<int[]> RequireInput
    {
      add { Controller.RequireInput += value; }
      remove { Controller.RequireInput -= value; }
    }

    public readonly Board Board;
    public readonly Team[] Teams;
    private readonly Controller Controller;

    public GameContext(GameSettings settings)
    {
      Settings = settings;
      Teams = new Team[settings.TeamCount];
      for (int i = 0; i < settings.TeamCount; i++)
        Teams[i] = new Team(i, settings);
      Board = new Board(this);
      Controller = new Controller(this);
    }

    public GameSettings Settings
    { get; private set; }

    private void OnGameEnd()
    {
      if (GameEnd != null)
        GameEnd(Teams[0].Pokemons.Values.Count((pm) => pm.Hp.Value > 0), Teams[1].Pokemons.Values.Count((pm) => pm.Hp.Value > 0));
    }
    public Pokemon GetPokemon(int id)
    {
      foreach (Team t in Teams)
        foreach (Pokemon pm in t.Pokemons.Values)
          if (pm.Id == id) return pm;
      return null;
    }
    public Player GetPlayer(int id)
    {
      foreach (Team t in Teams)
        foreach (Player p in t.Players)
          if (p.Id == id) return p;
      return null;
    }

    #region IGame Only
    bool IGame.Prepared
    {
      get
      {
        foreach (Team t in Teams)
          if (!t.Prepared) return false;
        return true;
      }
    }
    bool IGame.SetPlayer(int teamId, int userId, PokemonCustomInfo[] pokemons)
    {
      //TODO: Verify
      return Teams[teamId].AddPlayer(userId, pokemons);
    }
    bool IGame.Start()
    {
      if (((IGame)this).Prepared)
      {
        Controller.StartGameLoop(); //想用异步...
        return true;
      }
      return false;
    }
    bool IGame.InputAction(int playerId, ActionInput action)
    {
      return action.Input(Controller, GetPlayer(playerId));
    }
    ReportFragment IGame.GetLastLeapFragment() // for spectator
    {
      return Controller.ReportBuilder.GetLeapFragment(); //is null possible?
    }
    #endregion
  }
}
