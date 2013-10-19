using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Game.Host
{
  public class GameContext : IDisposable
  {
    private readonly Controller Controller;
    private bool gaming;

    internal GameContext(IGameSettings settings, Team[] teams)
    {
      _settings = settings;
      _teams = teams;
      Controller = new Controller(settings, teams);
    }

    public event Action<ReportFragment, IDictionary<int, InputRequest>> GameUpdated
    {
      add { Controller.GameUpdated += value; }
      remove { Controller.GameUpdated -= value; }
    }
    public event Action<IEnumerable<KeyValuePair<int, int>>> TimeUp
    {
      add { Controller.Timer.TimeUp += value; }
      remove { Controller.Timer.TimeUp -= value; }
    }
    public event Action<IEnumerable<int>> WaitingNotify
    {
      add { Controller.Timer.WaitingNotify += value; }
      remove { Controller.Timer.WaitingNotify -= value; }
    }

    private readonly Team[] _teams;
    public IEnumerable<Team> Teams
    { get { return _teams; } }
    private readonly IGameSettings _settings;
    public IGameSettings Settings
    { get { return _settings; } }

    public Player GetPlayer(int id)
    {
      foreach (Team t in _teams)
        foreach (Player p in t.Players)
          if (p.Id == id) return p;
      return null;
    }

    public void Start()
    {
      gaming = true;
      Controller.StartGameLoop(); //想用异步...
    }
    public void TryContinue()
    {
      Controller.TryContinueGameLoop();
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
    public bool InputAction(int playerId, ActionInput action)
    {
      if (gaming)
      {
        //action.Input(controller, )
        var player = GetPlayer(playerId);
        var inputs = action.Inputs;
        for (int x = 0; x < inputs.Length; ++x)
          if (inputs[x] != null)
          {
            if (Controller.GameSettings.Mode.GetPlayerIndex(x) != Controller.Teams[player.TeamId].GetPlayerIndex(player.Id)) return false;
            if (!Input(inputs[x], Controller, Controller.Board[player.TeamId][x])) return false;
          }
        return Controller.CheckInputSucceed(player);
      }
      return false;
    }
    public ReportFragment GetLastLeapFragment() // for spectator
    {
      return Controller.ReportBuilder.GetLeapFragment(); //is null possible?
    }

    public void Dispose()
    {
      Controller.Dispose();
    }
  }
}
