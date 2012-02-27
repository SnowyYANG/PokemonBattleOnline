using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LightStudio.PokemonBattle.Interactive
{
  [KnownType(typeof(OtherActionInputFailure))]
  [DataContract(Namespace = Namespaces.DEFAULT)]
  public abstract class ActionInputFailure
  {
    public abstract string GetMessage();
  }

  public class OtherActionInputFailure : ActionInputFailure
  {
    string Message;
    
    internal OtherActionInputFailure(string message = "")
    {
      Message = message;
    }

    public override string GetMessage()
    {
      return Message;
    }
  }
}
