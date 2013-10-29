using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Game.Host
{
  /// <summary>
  /// thread unsafe, do not access properties or methods concurrently
  /// </summary>
  public class GameContext : IDisposable
  {
    private readonly Controller Controller;
    private bool gaming;

    internal GameContext(IGameSettings settings, IPokemonData[,][] pokemons)
    {
      Controller = new Controller(settings, pokemons);
    }

    public event Action<ReportFragment, InputRequest[,]> GameUpdated
    {
      add { Controller.GameUpdated += value; }
      remove { Controller.GameUpdated -= value; }
    }
    public event Action<int[,]> TimeUp
    {
      add { Controller.Timer.TimeUp += value; }
      remove { Controller.Timer.TimeUp -= value; }
    }
    public event Action<bool[,]> WaitingNotify
    {
      add { Controller.Timer.WaitingNotify += value; }
      remove { Controller.Timer.WaitingNotify -= value; }
    }

    public IGameSettings Settings
    { get { return Controller.GameSettings; } }

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
    public bool InputAction(int teamId, int teamIndex, ActionInput action)
    {
      if (gaming)
      {
        //action.Input(controller, )
        var inputs = action.Inputs;
        for (int x = 0; x < inputs.Length; ++x)
          if (inputs[x] != null)
          {
            if (Controller.GameSettings.Mode.GetPlayerIndex(x) != teamIndex) return false;
            if (!Input(inputs[x], Controller, Controller.Board[teamId][x])) return false;
          }
        return Controller.CheckInputSucceed(teamId, teamIndex);
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
