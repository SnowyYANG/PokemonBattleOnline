using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Game.Host;
using PokemonBattleOnline.Network.Commands;

namespace PokemonBattleOnline.Network
{
    internal class RoomHost : IDisposable
    {
        public readonly PboServer Server;
        public readonly Room Room;
        internal readonly Dictionary<string, PboUser> Users = new Dictionary<string, PboUser>();
        private InitingGame initingGame;
        private GameContext game;

        public RoomHost(PboServer server, string id)
        {
            Server = server;
            Room = new Room(id);
        }

        private InitingGame rig;
        private void TryStartGame()
        {
            if (initingGame.CanComplete)
            {
                Room.Battling = true;
                Server.Send(this, RoomS2C.ChangeBattling(Room.Id));
                if (Room.Settings.Mode.PlayersPerTeam() == 2)
                {
                    Server.Send(Room[0, 0].Name, new PartnerInfoS2C(initingGame.GetPokemons(0, 1)));
                    Server.Send(Room[0, 1].Name, new PartnerInfoS2C(initingGame.GetPokemons(0, 0)));
                    Server.Send(Room[1, 0].Name, new PartnerInfoS2C(initingGame.GetPokemons(1, 1)));
                    Server.Send(Room[1, 1].Name, new PartnerInfoS2C(initingGame.GetPokemons(1, 0)));
                }
                rig = initingGame;
                game = initingGame.Complete();
                initingGame = null;
                game.GameUpdated += OnGameUpdate;
                game.GameEnd += EndGame;
                game.TimeUp += OnTimeUp;
                game.WaitingNotify += OnWaitingForInput;
                game.Error += OnError;
                Server.Send(this, new GameStartS2C(game.GetFragment()));
                game.Start();
            }
        }

        private void EndGame()
        {
            Record.Add(Room, rig);
            Record.Add(Room, game);
            game.Dispose();
            game = null;
            Room.Battling = false;
            Server.Send(this, RoomS2C.ChangeBattling(Room.Id));
        }
        private void OnError()
        {
            Record.Error(Room, game);
            EndGame();
            Server.Send(this, GameEndS2C.GameStop(null, GameStopReason.Error));
        }
        private void OnGameStop(string userId, GameStopReason reason)
        {
            Record.Add(Room, game, reason, userId);
            EndGame();
            Server.Send(this, GameEndS2C.GameStop(userId, reason));
        }
        private void OnTimeUp(int[,] time)
        {
            Record.Add(Room, game, "TimeUp");
            EndGame();
            var ps = new List<KeyValuePair<string, int>>(4);
            foreach (var p in Room.Players) ps.Add(new KeyValuePair<string, int>(p.Name, time[p.Seat.TeamId(), p.Seat.TeamIndex()]));
            Server.Send(this, GameEndS2C.TimeUp(ps.ToArray()));
        }
        private void OnWaitingForInput(bool[,] players)
        {
            var ps = new List<string>(4);
            foreach (var p in Room.Players)
                if (players[p.Seat.TeamId(), p.Seat.TeamIndex()]) ps.Add(p.Name);
            Server.Send(this, new WaitingForInputS2C(ps.ToArray()));
        }
        private void OnGameUpdate(GameEvent[] events, InputRequest[,] requirements)
        {
            if (requirements != null)
            {
                foreach (var p in Room.Players)
                {
                    var r = requirements[p.Seat.TeamId(), p.Seat.TeamIndex()];
                    if (r != null) Server.Send(p.Name, new RequireInputS2C(r));
                }
            }
            Server.Send(this, new GameUpdateS2C(events));
        }

        private bool IsPrepared(Seat seat)
        {
            return initingGame != null && seat != Seat.Spectator && initingGame.GetPokemons(seat.TeamId(), seat.TeamIndex()) != null;
        }
        private int GameId;
        public void Prepare(PboUser su, GameSettings settings, IPokemonData[] pokemons)
        {
            if (game == null)
            {
                if (initingGame == null) initingGame = new InitingGame(++GameId, settings);
                var seat = su.User.Seat;
                if (initingGame.Prepare(seat.TeamId(), seat.TeamIndex(), pokemons))
                {
                    Server.Send(this, new SetPrepareS2C(seat, true));
                    Room.Settings = settings;
                    TryStartGame();
                }
            }
        }
        public void UnPrepare(PboUser su)
        {
            if (game == null)
            {
                var seat = su.User.Seat;
                if (IsPrepared(seat))
                {
                    initingGame.UnPrepare(seat.TeamId(), su.User.Seat.TeamIndex());
                    Server.Send(this, new SetPrepareS2C(seat, false));
                }
            }
        }

        public bool AddUser(PboUser su, Seat seat)
        {
            var user = su.User;
            if (seat == Seat.Spectator || Room[seat] == null)
            {
                su.Room = this;
                user.RoomId = Room.Id;
                if (seat == Seat.Spectator) Room.AddSpectator(user);
                else Room[seat] = user;
                Users.Add(su.User.Name, su);
                //Server.Send(SetSeatS2C.InRoom(user));
                if (game != null) Server.Send(su.User.Name, new GameStartS2C(game.GetFragment()));
                else if (initingGame != null)
                {
                    if (IsPrepared(Seat.Player00)) Server.Send(su.User.Name, new SetPrepareS2C(Seat.Player00, true));
                    if (IsPrepared(Seat.Player10)) Server.Send(su.User.Name, new SetPrepareS2C(Seat.Player10, true));
                    if (IsPrepared(Seat.Player01)) Server.Send(su.User.Name, new SetPrepareS2C(Seat.Player01, true));
                    if (IsPrepared(Seat.Player11)) Server.Send(su.User.Name, new SetPrepareS2C(Seat.Player11, true));
                }
                return true;
            }
            return false;
        }
        public void RemoveUser(PboUser su)
        {
            var id = su.User.Name;
            var seat = su.User.Seat;
            if (seat == Seat.Spectator)
            {
                Users.Remove(su.User.Name);
                Room.RemoveSpectator(su.User);
                //Send(SetSeatS2C.LeaveRoom(id));
            }
            else if (Room.Players.Count() == 1) Server.RemoveRoom(this);
            else
            {
                if (game != null) OnGameStop(id, GameStopReason.PlayerStop);
                else UnPrepare(su);
                Users.Remove(id);
                Room[seat] = null;
                //Server.Send(SetSeatS2C.LeaveRoom(id));
            }
        }

        public void Input(PboUser su, ActionInput action)
        {
            var seat = su.User.Seat;
            if (game != null)
                if (game.InputAction(seat.TeamId(), seat.TeamIndex(), action)) game.TryContinue();
                else OnGameStop(su.User.Name, GameStopReason.InvalidInput);
        }

        public void Dispose()
        {
            if (game != null) game.Dispose();
        }
    }
}
