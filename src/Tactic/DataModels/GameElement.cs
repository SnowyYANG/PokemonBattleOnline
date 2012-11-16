using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LightStudio.Tactic.DataModels
{
  [DataContract(Namespace = Namespaces.PBO)]
  public abstract class GameElement
  {
    [DataMember]
    public int Id { get; private set; }

    public GameElement(int id)
    {
      Id = id;
    }

    [DataMember]
    public string Name { get; protected set; }

    public abstract string Description { get; }

    public override string ToString()
    {
      return Name;
    }
  }
}
