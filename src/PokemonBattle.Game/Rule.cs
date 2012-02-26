using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.Tactic.DataModels;

namespace LightStudio.PokemonBattle.Game
{
  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class Rule : ExecutableGameElement<IController>
  {
    public Rule(int id) : base(id)
    {
    }
  }
}
