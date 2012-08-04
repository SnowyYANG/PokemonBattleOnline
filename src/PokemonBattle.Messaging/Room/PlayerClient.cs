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
    private readonly int teamId;
    private readonly PokemonCustomInfo[] pokemons;
    private SimGame game;

    public PlayerClient(int hostId, int teamId, PokemonCustomInfo[] pokemons)
      : base(hostId)
    {
      this.teamId = teamId;
      this.pokemons = pokemons;
    }

    public override Tactic.Messaging.UserState State
    { get { return Tactic.Messaging.UserState.Battling; } }
    public override IPlayerController PlayerController
    { get { return this; } }

    protected override void InformEnterSucceed(GameInitSettings settings, Player[] players, int[] spectators)
    {
      base.InformEnterSucceed(settings, players, spectators);
    }
    protected override void OnGameStarted()
    {
      game = new SimGame(PBOClient.Client.User.Id, teamId, pokemons, Settings, Settings.NextId);
    }

    #region IPlayerController
    private Action<RequireInput> _requireInput;
    event Action<RequireInput> IPlayerController.RequireInput
    {
      add { _requireInput += value; }
      remove { _requireInput -= value; }
    }
    Game.Player IPlayerController.Player
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
    protected override void InformRequireInput(Game.RequireInput info)
    {
      //game.Update(info); //not thread safe, solution: requireInput come before reportUpdate
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
