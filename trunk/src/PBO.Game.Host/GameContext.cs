﻿using System;
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
    public event Action GameEnd
    {
      add { Controller.GameEnd += value; }
      remove { Controller.GameEnd -= value; }
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
    public event Action Error;

    public IGameSettings Settings
    { get { return Controller.GameSettings; } }

    public void Start()
    {
      try
      {
        gaming = true;
        Controller.StartGameLoop(); //想用异步...
      }
      catch
      {
        Error();
      }
    }
    public void TryContinue()
    {
      try
      {
        Controller.TryContinueGameLoop();
      }
      catch
      {
        Error();
      }
    }
    private bool Input(XActionInput input, Controller controller, Tile tile)
    {
      bool r = false;
      try
      {
        if (input.SendOutIndex > 0) r = controller.InputSendOut(tile, input.SendOutIndex);
        else
        {
          var pm = tile.Pokemon;
          if (input.Move > 0)
          {
            foreach (MoveProxy m in pm.Moves)
              if (m.Type.Id == input.Move)
              {
                Tile target = input.TargetTeam > 0 ? controller.Board[input.TargetTeam - 1][input.TargetX - 1] : null;
                r = controller.InputSelectMove(m, target, input.Mega);
                break;
              }
          }
          else r = controller.InputStruggle(pm);
        }
      }
      catch
      {
        Error();
      }
      return r;
    }
    public bool InputAction(int teamId, int teamIndex, ActionInput action)
    {
      try
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
      }
      catch
      {
        Error();
      }
      return false;
    }
    public ReportFragment GetLastLeapFragment() // for spectator
    {
      return Controller.ReportBuilder.GetLeapFragment(); //is null possible?
    }

    private bool _isDisposed;
    public void Dispose()
    {
      if (!_isDisposed)
      {
        _isDisposed = true;
        Controller.Dispose();
      }
    }
  }
}
