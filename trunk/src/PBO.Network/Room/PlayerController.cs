using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Network
{
  public class PlayerController
  {
    public event Action<InputRequest> RequireInput;

    private readonly Client Client;
    internal PlayerController(RoomController room, IPokemonData[] pokemons, IPokemonData[] parner)
    {
      Client = room.Client;
      _game = new SimGame(room.Room.Settings, new SimPlayer(room.User.Seat.TeamId(), room.User.Seat.TeamIndex(), pokemons), parner);
    }

    public SimPlayer Player
    { get { return _game.Player; } }
    private readonly SimGame _game;
    public SimGame Game
    { get { return _game; } }

    public void Input(ActionInput input)
    {
      Client.Send(new Commands.InputC2S(input));
    }

    internal void OnRequireInput(InputRequest inputRequest)
    {
      RequireInput(inputRequest);
    }
  }
}
