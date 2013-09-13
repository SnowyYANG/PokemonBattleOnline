using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Game.Host
{
  internal class GameContext : IGame
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
    private bool Input(XActionInput input, Controller controller, Tile tile)
    {
      bool r = false;
      if (input.SendoutIndex > 0) r = controller.InputSendout(tile, input.SendoutIndex);
      else
      {
        var pm = tile.Pokemon;
        if (input.Move > 0)
        {
          foreach (MoveProxy m in pm.Moves)
            if (m.Type.Id == input.Move)
            {
              Tile target = input.TargetTeam > 0 ? controller.Board[input.TargetTeam - 1][input.TargetX - 1] : null;
              r = controller.InputSelectMove(m, target);
              break;
            }
        }
        else r = controller.InputStruggle(pm);
      }
      return r;
    }
    bool IGame.InputAction(int playerId, ActionInput action)
    {
      if (gaming)
      {
        //action.Input(controller, )
        var player = GetPlayer(playerId);
        var inputs = action.Inputs;
        for (int x = 0; x < inputs.Length; ++x)
          if (inputs[x] != null)
          {
            if (controller.Game.Settings.Mode.GetPlayerIndex(x) != controller.Game.Teams[player.TeamId].GetPlayerIndex(player.Id)) return false;
            if (!Input(inputs[x], controller, controller.Board[player.TeamId][x])) return false;
          }
        return controller.CheckInputSucceed(player);
      }
      return false;
    }
    ReportFragment IGame.GetLastLeapFragment() // for spectator
    {
      return controller.ReportBuilder.GetLeapFragment(); //is null possible?
    }
    #endregion
  }
}
