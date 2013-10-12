using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace PokemonBattleOnline.Network
{
  public class Room : ObservableObject
  {
    public int Id;
    public ObservableList<User> Spectators;
    public GameInitSettings Settings;
    public bool ReadyA1;
    public bool ReadyA2;
    public bool ReadyB1;
    public bool ReadyB2;

    public Room(int id, GameInitSettings settings, User a1)
    {
      Id = id;
      this[0, 0] = a1;
      Spectators = new ObservableList<User>();
    }

    private static PropertyChangedEventArgs BATTLING = new PropertyChangedEventArgs("Battling");
    private bool _battling;
    public bool Battling
    {
      get { return _battling; }
      set
      {
        if (_battling != value)
        {
          _battling = value;
          OnPropertyChanged(BATTLING);
        }
      }
    }

    public User[] players;
    public User this[Seat seat]
    {
      get { return players.ValueOrDefault((int)seat); }
      set { if (seat != Seat.Spectator) players[(int)seat] = value; }
    }
    public User this[int teamIndex, int playerIndex]
    {
      get { return players.ValueOrDefault(teamIndex * 2 + playerIndex); }
      set { players[teamIndex * 2 + playerIndex] = value; }
    }
  }
}
