using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LightStudio.PokemonBattle.Messaging.Room
{
  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class Player
  {
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

    [DataMember(EmitDefaultValue = false)]
    public bool IsInputing
    { get; private set; }
    [DataMember(EmitDefaultValue = false)]
    public int Seconds
    { get; private set; }

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
