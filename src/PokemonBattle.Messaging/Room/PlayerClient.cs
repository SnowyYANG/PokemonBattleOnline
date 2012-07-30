using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;
using LightStudio.Tactic.Messaging.Lobby;

namespace LightStudio.PokemonBattle.Messaging.Room
{
  internal class PlayerClient : RoomUserBase, IPlayerController
  {
    private readonly List<IPlayerControllerEvents> listeners;
    private readonly int teamId;
    private readonly PokemonCustomInfo[] pokemons;
    private SimGame game;

    public PlayerClient(int hostId, int teamId, PokemonCustomInfo[] pokemons)
      : base(hostId)
    {
      listeners = new List<IPlayerControllerEvents>();
      this.teamId = teamId;
      this.pokemons = pokemons;
    }

    public override Tactic.Messaging.UserState State
    { get { return Tactic.Messaging.UserState.Battling; } }
    public override IPlayerController PlayerController
    { get { return this; } }

    protected override void InformEnterSucceed(GameInitSettings settings, int[] players, int[] spectators)
    {
      base.InformEnterSucceed(settings, players, spectators);
      game = new SimGame(PBOClient.Client.User.Id, teamId, pokemons, Game.Settings, settings.NextId);
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
    protected override void InformReportAddition(Game.PokemonAdditionalInfo info)
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

    public override void EnterRoom()
    {
      sendCommand(new JoinGameCommand(pokemons, teamId));
    }
  }
}
