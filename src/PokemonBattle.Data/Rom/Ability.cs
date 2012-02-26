using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.Tactic.DataModels;

namespace LightStudio.PokemonBattle.Data
{
  [DataContract(Namespace=Namespaces.DEFAULT)]
  public class Ability : GameElement
  {
    public Ability(int id)
      : base(id)
    {
    }
  }
}
