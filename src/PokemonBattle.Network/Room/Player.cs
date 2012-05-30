using System;
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
    }

    public override UserRole Role
    { get { return UserRole.Player; } }
    public override IPlayerController PlayerController
    { get { return this; } }

    protected override void InformEnterSucceed(GameInitSettings settings, int[] players, int[] spectators)
    {
      base.InformEnterSucceed(settings, players, spectators);
      game = new SimGame(LobbyService.User.Id, teamId, pokemons, Game.Settings, settings.NextId);
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

    #region Update
    protected override void InformReportUpdate(ReportFragment fragment)
    {
      base.InformReportUpdate(fragment);
      UIDispatcher.Invoke(() =>
        {
          if (game.Update(fragment))
            foreach (IPlayerControllerEvents listner in listeners) listner.RequireInput();
        });
    }
    protected override void InformPmAdditional(Interactive.PokemonAdditionalInfo info)
    {
      game.Update(info);
    }
    #endregion

    #region Tie
    protected override void InformRequestTie()
    {
    }
    protected override void InformTieRejected()
    {
    }
    #endregion

    #region Input
    protected override void InformInputResult(bool ok, string message, bool allDone)
    {
      foreach (IPlayerControllerEvents l in listeners)
        l.InputResult(ok, message, allDone);
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
