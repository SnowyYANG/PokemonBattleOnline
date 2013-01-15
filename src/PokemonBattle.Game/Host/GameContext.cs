using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Data;

namespace PokemonBattleOnline.Game.Host
{
  public class GameContext : IGame
  {
    public readonly Board Board;
    private readonly Controller controller;
    private bool gaming;

    internal GameContext(IGameSettings settings, Team[] teams)
    {
      _settings = settings;
      _teams = teams;
      Board = new Board(this);
      controller = new Controller(this);
    }

    private readonly Team[] _teams;
    public Team[] Teams
    { get { return _teams; } }
    IEnumerable<Team> IGame.Teams
    { get { return _teams; } }
    private readonly IGameSettings _settings;
    public IGameSettings Settings
    { get { return _settings; } }
    public int Turn
    { get { return controller.TurnNumber; } }

    public bool CheckGameEnd()
    {
      if (Teams.Any((t) => t.Players.All((p) => p.PmsAlive == 0)))
      {
        gaming = false;
        return true;
      }
      return false;
    }
    public Player GetPlayer(int id)
    {
      foreach (Team t in Teams)
        foreach (Player p in t.Players)
          if (p.Id == id) return p;
      return null;
    }

    #region IGame Only
    event Action<ReportFragment, IDictionary<int, InputRequest>> IGame.ReportUpdated
    {
      add { controller.ReportUpdated += value; }
      remove { controller.ReportUpdated -= value; }
    }

    void IGame.Start()
    {
      gaming = true;
      controller.StartGameLoop(); //想用异步...
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
