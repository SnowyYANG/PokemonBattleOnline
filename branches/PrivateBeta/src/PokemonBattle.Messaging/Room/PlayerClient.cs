using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;
using LightStudio.Tactic.Messaging.Lobby;

namespace LightStudio.PokemonBattle.Messaging.Room
{
  internal class PlayerClient : RoomUserClient, IPlayerController
  {
    private readonly int TeamId;
    private readonly IPokemonData[] Pokemons;
    private SimGame game;

    public PlayerClient(int hostId, int teamId, IPokemonData[] pokemons)
      : base(hostId)
    {
      this.TeamId = teamId;
      this.Pokemons = pokemons;
    }

    public override Tactic.Messaging.UserState State
    { get { return Tactic.Messaging.UserState.Battling; } }
    public override IPlayerController PlayerController
    { get { return this; } }

    protected override void InformEnterSucceed(GameInitSettings settings, Player[] players, int[] spectators)
    {
      base.InformEnterSucceed(settings, players, spectators);
    }
    protected override void InformPlayerInfo(int teamIndex, IPokemonData[] parner)
    {
      var player = new SimPlayer(PBOClient.Client.User.Id, TeamId, teamIndex, Pokemons);
      game = new SimGame(Settings, player, parner);
    }

    #region IPlayerController
    private Action<InputRequest, int> _requireInput;
    event Action<InputRequest, int> IPlayerController.RequireInput
    {
      add { _requireInput += value; }
      remove { _requireInput -= value; }
    }
    SimPlayer IPlayerController.Player
    { get { return game.Player; } }
    SimGame IPlayerController.Game
    { get { return game; } }

    void IPlayerController.Input(ActionInput input)
    {
      sendCommand(new InputCommand(input));
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
    #endregion

    #region Update
    private InputRequest inputRequest;
    private int spentTime;
    protected override void InformReportUpdate(ReportFragment fragment)
    {
      base.InformReportUpdate(fragment);
      UIDispatcher.Invoke(() =>
        {
          game.Update(fragment);
          if (inputRequest != null)
          {
            _requireInput(inputRequest, spentTime);
            inputRequest = null;
          }
        });
    }
    protected override void InformRequireInput(InputRequest request, int spentTime)
    {
      inputRequest = request;
      this.spentTime = spentTime;
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
      sendCommand(new JoinGameCommand(Pokemons, TeamId));
    }
  }
}
