using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Data
{
  [DataContract(Namespace=Namespaces.PBO)]
  public class Ability : GameElement
  {
    private Ability(int id)
      : base(id)
    {
    }
    public override string Name
    { get { return DataService.DataString[EnglishName]; } }
    public override string Description
    { get { return DataService.DataString["A" + Id.ToString("000")]; } }
  }
}
