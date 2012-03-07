﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;
using LightStudio.PokemonBattle.Interactive;
using LightStudio.Tactic.Messaging.Lobby;

namespace LightStudio.PokemonBattle.Room
{
  internal class Player : User, IPlayerController
  {
    private readonly List<IPlayerControllerEvents> listeners;
    private SimGame game;
    private int teamId;
    private PokemonCustomInfo[] pokemons;

    public Player(int hostId)
      : base(hostId)
    {
      listeners = new List<IPlayerControllerEvents>();
      EnterSucceed += (uc) => game = new SimGame(LobbyService.User.Id, teamId, pokemons, Game.Settings);
    }

    public override UserRole Role
    { get { return UserRole.Player; } }
    public override IPlayerController PlayerController
    { get { return this; } }
    protected void Input(int userId, ActionInput action)
    {
    }

    #region IPlayerController
    Game.Player IPlayerController.Player
    { get { return game.Player; } }
    SimGame IPlayerController.Game
    { get { return game; } }

    bool IPlayerController.UseMove(SimMove move, Tile target)
    {
      //TODO: verify
      sendCommand(new InputCommand(ActionInput.UseMove(move, target)));
      return true;
    }
    bool IPlayerController.Switch(SimPokemon withdraw, Pokemon sendout)
    {
      sendCommand(new InputCommand(ActionInput.Switch(withdraw, sendout)));
      return true;
    }
    bool IPlayerController.Struggle(SimPokemon pm)
    {
      sendCommand(new InputCommand(ActionInput.Struggle(pm)));
      return true;
    }
    void IPlayerController.Quit()
    {
      sendCommand(new QuitCommand());
    }
    bool IPlayerController.RequestTie()
    {
      return false;
    }
    bool IPlayerController.RejectTie()
    {
      return false;
    }
    bool IPlayerController.AcceptTie()
    {
      return false;
    }
    void IPlayerController.AddEventsListener(IPlayerControllerEvents listner)
    {
      listeners.Add(listner);
    }
    #endregion

    protected override void InformTurn(ReportFragment turn)
    {
      base.InformTurn(turn);
      game.Update(turn);
    }
    protected override void InformPmAdditional(Interactive.PokemonAdditionalInfo info)
    {
      game.Update(info);
    }

    #region Tie
    protected override void InformRequestTie()
    {
    }
    protected override void InformTieRejected()
    {
    }
    #endregion

    #region Input
    protected override void InformRequireInput()
    {
      foreach(IPlayerControllerEvents l in listeners)
        l.RequireInput();
    }
    protected override void InformInputFail()
    {
      foreach (IPlayerControllerEvents l in listeners)
        l.InputFailed();
    }
    protected override void InformInputSucceed()
    {
      foreach (IPlayerControllerEvents l in listeners)
        l.InputSucceeded();
    }
    #endregion

    public void JoinGame(PokemonCustomInfo[] info, int teamId)
    {
      this.pokemons = info;
      this.teamId = teamId;
      sendCommand(new JoinGameCommand(info, teamId));
    }
  }
}
