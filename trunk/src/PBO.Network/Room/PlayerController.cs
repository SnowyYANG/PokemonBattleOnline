using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Network.Commands;

namespace PokemonBattleOnline.Network
{
  public class PlayerController
  {
    public event Action<InputRequest> RequireInput;

    private readonly Client Client;

    internal PlayerController(RoomController room, IPokemonData[] pokemons, IPokemonData[] parner)
    {
      Client = room._Client;
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
    public void GiveUp()
    {
      Client.Send(new Commands.GiveUpC2S());
    }

    internal void OnRequireInput(InputRequest inputRequest)
    {
#if TEST
      if (RequireInput != null)
#endif
        RequireInput(inputRequest);
    }
  }
}
