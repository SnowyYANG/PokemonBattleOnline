using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LightStudio.Tactic.DataModels
{
  [DataContract(Namespace = Namespaces.PBO)]
  public class GameElement
  {
    public const string DESCRIPTION_FORMAT = "{0}_Des";

    [DataMember]
    public int Id { get; private set; }

    public GameElement(int id)
    {
      Id = id;
    }

    [DataMember]
    public string Name { get; protected set; }

    public string Description
    {
      get
      {
        return string.Format(DESCRIPTION_FORMAT, Name);
      }
    }

    public override string ToString()
    {
      return Name;
    }
  }
}
