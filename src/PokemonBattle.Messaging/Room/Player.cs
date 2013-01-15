using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Messaging.Room
{
  [DataContract(Namespace = Namespaces.JSON)]
  public class Player
  {
    [DataMember(Name = "a")]
    public readonly int Id;
    [DataMember(Name = "b", EmitDefaultValue = false)]
    public readonly int Team;

    public Player(int id, int team)
    {
      Id = id;
      Team = team;
    }

    public string Name
    { get; internal set; }
  }
}
