using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Game.Host;
using PokemonBattleOnline.Network.Commands;

namespace PokemonBattleOnline.Network
{
  internal class RoomController : IDisposable
  {
    public readonly Server Server;
    public readonly Room Room;
    private readonly Dictionary<int, ServerUser> Users;
    private InitingGame initingGame;
    private GameContext game;
    
    public RoomController(Server server, int id, string name, GameSettings settings)
    {
      Server = server;
      Room = new Room(id, name, settings);
      Users = new Dictionary<int, ServerUser>();
    }

    public void Send(IS2C s2c)
    {
      foreach(var su in Users.Values) su.Send(s2c);
    }

    private void TryStartGame()
    {
      if (initingGame.CanComplete)
      {
        Room.Battling = true;
        Server.Send(RoomS2C.ChangeBattling(Room.Id));
        if (Room.Settings.Mode.PlayersPerTeam() == 2)
        {
          Server.GetUser(Room[0, 0].Id).Send(new PartnerInfoS2C(initingGame.GetPokemons(0, 1)));
          Server.GetUser(Room[0, 1].Id).Send(new PartnerInfoS2C(initingGame.GetPokemons(0, 0)));
          Server.GetUser(Room[1, 0].Id).Send(new PartnerInfoS2C(initingGame.GetPokemons(1, 1)));
          Server.GetUser(Room[1, 1].Id).Send(new PartnerInfoS2C(initingGame.GetPokemons(1, 0)));
        }
        game = initingGame.Complete();
        initingGame = null;
        game.GameUpdated += OnGameUpdate;
        game.TimeUp += OnTimeUp;
        game.WaitingNotify += OnWaitingForInput;
        game.Start();
      }
    }

    private void EndGame()
    {
      game.Dispose();
      game = null;
      Room.Battling = false;
      Server.Send(RoomS2C.ChangeBattling(Room.Id));
    }
    private void OnGameStop(int userId, GameStopReason reason)
    {
      EndGame();
      Send(GameEndS2C.GameStop(userId, reason));
    }
    private void OnTimeUp(int[,] time)
    {
      EndGame();
      Send(GameEndS2C.TimeUp(Room.Players.Select((p) => new KeyValuePair<int, int>(p.Id, time[p.Seat.TeamId(), p.Seat.TeamIndex()]))));
    }
    private void OnWaitingForInput(bool[,] players)
    {
      Send(new WaitingForInputS2C(Room.Players.Where((p) => players[p.Seat.TeamId(), p.Seat.TeamIndex()]).Select((p) => p.Id).ToArray()));
    }
    private void OnGameUpdate(ReportFragment fragment, InputRequest[,] requirements)
    {
      if (requirements != null)
      {
        foreach(var p in Room.Players)
        {
          var r = requirements[p.Seat.TeamId(), p.Seat.TeamIndex()];
          if (r != null) Server.GetUser(p.Id).Send(new RequireInputS2C(r));
        }
      }
      Send(new GameUpdateS2C(fragment));
    }

    private bool IsPrepared(Seat seat)
    {
      return initingGame != null && initingGame.GetPokemons(seat.TeamId(), seat.TeamIndex()) != null;
    }
    public void Prepare(ServerUser su, IPokemonData[] pokemons)
    {
      if (game == null)
      {
        if (initingGame == null) initingGame = new InitingGame(Room.Settings);
        var seat = su.User.Seat;
        if (initingGame.Prepare(seat.TeamId(), seat.TeamIndex(), pokemons))
        {
          Send(new SetPrepare(seat, true));
          TryStartGame();
        }
      }
    }
    public void UnPrepare(ServerUser su)
    {
      if (game == null)
      {
        var seat = su.User.Seat;
        if (IsPrepared(seat))
        {
          initingGame.UnPrepare(seat.TeamId(), su.User.Seat.TeamIndex());
          Send(new SetPrepare(seat, false));
        }
      }
    }

    public void AddUser(ServerUser su, Seat seat)
    {
      var user = su.User;
      if (seat == Seat.Spectator) Room.AddSpectator(user);
      else if (Room[seat] == null) Room[seat] = user;
      else return;
      Users.Add(user.Id, su);
      Server.Send(SetSeatS2C.EnterRoom(user.Id, Room.Id, seat));
      if (game != null) su.Send(new GameUpdateS2C(game.GetLastLeapFragment()));
      else if (initingGame != null)
      {
        if (IsPrepared(Seat.Player00)) su.Send(new SetPrepare(Seat.Player00, true));
        if (IsPrepared(Seat.Player10)) su.Send(new SetPrepare(Seat.Player10, true));
        if (Room.Settings.Mode.PlayersPerTeam() == 2)
        {
          if (IsPrepared(Seat.Player01)) su.Send(new SetPrepare(Seat.Player01, true));
          if (IsPrepared(Seat.Player11)) su.Send(new SetPrepare(Seat.Player11, true));
        }
      }
    }
    public void RemoveUser(ServerUser su)
    {
      var id = su.User.Id;
      var seat = su.User.Seat;
      if (seat == Seat.Spectator)
      {
        Users.Remove(id);
        Room.RemoveSpectator(su.User);
        Server.Send(SetSeatS2C.LeaveRoom(id));
      }
      else if (Room.Players.Count() == 1) Server.RemoveRoom(this);
      else
      {
        if (game != null) OnGameStop(id, GameStopReason.PlayerGiveUp);
        else UnPrepare(su);
        Users.Remove(id);
        Room[seat] = null;
        Server.Send(SetSeatS2C.LeaveRoom(id));
      }
    }
    public void ChangeSeat(ServerUser su, Seat seat)
    {
      //if there is only one player, s/he cannot spectate
      //prepared player cannot change seat
      //no one can change seat while room is battling
      throw new NotImplementedException();
    }

    public void Input(ServerUser su, ActionInput action)
    {
      var seat = su.User.Seat;
      if (game != null)
        if (game.InputAction(seat.TeamId(), seat.TeamIndex(), action)) game.TryContinue();
        else OnGameStop(su.User.Id, GameStopReason.InvalidInput);
    }

    public void Dispose()
    {
      if (game != null) game.Dispose();
    }
  }
}
