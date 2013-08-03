using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace LightStudio.PokemonBattle.Messaging.Room
{
  [DataContract(Namespace = PBOMarks.PBO)]
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
    }
  }
}
