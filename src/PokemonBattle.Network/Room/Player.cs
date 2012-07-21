using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;
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

    void IPlayerController.UseMove(byte x, SimMove move, int targetTeam, int targetX)
    {
      sendCommand(new InputCommand(ActionInput.UseMove(x, move, targetTeam, targetX)));
    }
    void IPlayerController.UseMove(byte x, SimMove move)
    {
      //TODO: verify
      sendCommand(new InputCommand(ActionInput.UseMove(x, move)));
    }
    void IPlayerController.Sendout(byte x, Pokemon sendout)
    {
      sendCommand(new InputCommand(ActionInput.Switch(x, sendout)));
    }
    void IPlayerController.Struggle(byte x)
    {
      sendCommand(new InputCommand(ActionInput.Struggle(x)));
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
    protected override void InformPmAdditional(Game.PokemonAdditionalInfo info)
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
    protected override void InformInputResult(bool ok, string messageKey, bool allDone)
    {
      foreach (IPlayerControllerEvents l in listeners)
        l.InputResult(ok, messageKey, allDone);
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
