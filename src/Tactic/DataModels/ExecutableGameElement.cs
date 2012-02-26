using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LightStudio.Tactic.DataModels
{
  [DataContract(Namespace=Namespaces.DEFAULT)]
  public class ExecutableGameElement<T> : GameElement
  {
    public ExecutableGameElement(int id)
      : base(id)
    {
    }

    public IExecutableElement<T> E { get; protected set; }
  }
}
