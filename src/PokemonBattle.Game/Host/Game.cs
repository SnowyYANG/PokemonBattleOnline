using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Host
{
  /// <summary>
  /// 这必须是单线程的...
  /// </summary>
  public class GameContext : IGame
  {
    public readonly Board Board;
    public readonly Team[] Teams;
    private readonly IGameSettings gameSettings;
    private Controller controller;
    private Action<int, int> gameEnd;
    private bool gaming;

    internal GameContext(IGameSettings settings, Func<int> nextId)
    {
      gameSettings = settings;
      Teams = new Team[settings.Mode.TeamCount()];
      for (int i = 0; i < Teams.Length; i++)
        Teams[i] = new Team(i, settings, nextId);
      Board = new Board(this);
    }

    public IGameSettings Settings
    { get { return gameSettings; } }
    public int Turn
    { get { return controller.TurnNumber; } }

    public bool CheckGameEnd()
    {
      int t0 = Teams[0].Pokemons.Values.Count((pm) => pm.Hp.Value > 0);
      int t1 = Teams[1].Pokemons.Values.Count((pm) => pm.Hp.Value > 0);
      if (t0 == 0 || t1 == 0)
      {
        gaming = false;
        return true;
      }
      return false;
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
    private Action<ReportFragment, IDictionary<int, InputRequest>> _reportUpdated;
    event Action<ReportFragment, IDictionary<int, InputRequest>> IGame.ReportUpdated
    {
      add { _reportUpdated += value; }
      remove { _reportUpdated -= value; }
    }

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
      //TODO: Verify both userId and pm
      return teamId >= 0 && teamId < Teams.Length && GetPlayer(userId) == null && Teams[teamId].AddPlayer(userId, pokemons) != null;
    }
    bool IGame.Start()
    {
      if (((IGame)this).Prepared)
      {
        gaming = true;
        controller = new Controller(this);
        controller.ReportUpdated += _reportUpdated;
        controller.StartGameLoop(); //想用异步...
        return true;
      }
      return false;
    }
    void IGame.TryContinue()
    {
      controller.TryContinueGameLoop();
    }
    bool IGame.InputAction(int playerId, ActionInput action)
    {
      return gaming && action.Input(controller, GetPlayer(playerId));
    }
    ReportFragment IGame.GetLastLeapFragment() // for spectator
    {
      return controller.ReportBuilder.GetLeapFragment(); //is null possible?
    }
    #endregion
  }
}
