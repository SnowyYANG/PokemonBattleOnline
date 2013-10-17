using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Network
{
  [DataContract(Namespace = PBOMarks.JSON)]
  public class Room : ObservableObject
  {
    [DataMember(Name = "a")]
    public int Id;

    public Room(int id, GameInitSettings settings)
    {
      Id = id;
      settings = _settings;
      Players = new User[4];
      _spectators = new ObservableList<User>();
    }

    [DataMember(Name = "b")]
    private readonly GameInitSettings _settings;
    public GameInitSettings Settings
    { get { return _settings; } }

    private static PropertyChangedEventArgs BATTLING = new PropertyChangedEventArgs("Battling");
    [DataMember(Name = "c", EmitDefaultValue = false)]
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

    public User this[Seat seat]
    {
      get { return Players.ValueOrDefault((int)seat); }
      set { if (seat != Seat.Spectator) Players[(int)seat] = value; }
    }
    public User this[int teamIndex, int playerIndex]
    {
      get { return Players.ValueOrDefault(teamIndex * 2 + playerIndex); }
      set { Players[teamIndex * 2 + playerIndex] = value; }
    }

    internal User[] Players
    { get; private set; }

    private ObservableList<User> _spectators;
    public ObservableList<User> Spectators
    { 
      get
      {
        if (_spectators == null) _spectators = new ObservableList<User>();
        return _spectators;
      }
    }

    private void SetPij(int i, int j, User value)
    {
      if (Players == null) Players = new User[4];
      value.Room = this;
      value.Seat = (Seat)(i * 2 + j);
      this[i, j] = value;
    }
    [DataMember(EmitDefaultValue = false, Order = 0)]
    private User p00
    {
      get { return this[0, 0]; }
      set { SetPij(0, 0, value); }
    }
    [DataMember(EmitDefaultValue = false, Order = 1)]
    private User p01
    {
      get { return this[0, 1]; }
      set { SetPij(0, 1, value); }
    }
    [DataMember(EmitDefaultValue = false, Order = 2)]
    private User p10
    {
      get { return this[1, 0]; }
      set { SetPij(1, 0, value); }
    }
    [DataMember(EmitDefaultValue = false, Order = 3)]
    private User p11
    {
      get { return this[1, 1]; }
      set { SetPij(1, 1, value); }
    }
    [DataMember(EmitDefaultValue = false, Order = 4)]
    private User[] ss
    {
      get { return _spectators.Any() ? _spectators.ToArray() : null; }
      set
      {
        if (value != null)
        {
          foreach (var u in value)
          {
            u.Room = this;
            u.Seat = Seat.Spectator;
          }
          _spectators = new ObservableList<User>(value);
        }
      }
    }
  }
}
