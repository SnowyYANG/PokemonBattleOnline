using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace LightStudio.PokemonBattle.Messaging.Room
{
  [DataContract(Namespace = Namespaces.LIGHT)]
  public class Player : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;
    [DataMember]
    public readonly int Id;
    [DataMember(EmitDefaultValue = false)]
    public readonly int Team;

    public Player(int id, int team)
    {
      Id = id;
      Team = team;
      Seconds = 1800;
    }

    private bool _isInputing;
    [DataMember(EmitDefaultValue = false)]
    public bool IsInputing
    {
      get { return _isInputing; }
      internal set
      {
        if (_isInputing != value)
        {
          _isInputing = value;
          SendPropertyChanged("IsInputing");
        }
      }
    }
    private int _seconds;
    [DataMember(EmitDefaultValue = false)]
    public int Seconds
    {
      get { return _seconds; }
      private set
      {
        if (_seconds != value)
        {
          _seconds = value;
          SendPropertyChanged("Seconds");
        }
      }
    }

    private void SendPropertyChanged(string propertyName)
    {
      if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }

    internal void NewTurn()
    {
      Seconds -= 20;
    }
    internal void Tick()
    {
      if (IsInputing) Seconds++;
    }
  }
}
