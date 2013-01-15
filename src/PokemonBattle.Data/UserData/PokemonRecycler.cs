using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Data
{
  [CollectionDataContract(Namespace = Namespaces.PBO, ItemName = "PokemonData")]
  public class PokemonRecycler : PokemonCollection
  {
    internal PokemonRecycler()
    {
    }

    public override string Name
    { 
      get { return DataService.String["Recycler"]; }
      set { }
    }
    public override bool CanAdd
    { get { return false; } }

    protected override void InsertItem(int index, PokemonData item)
    {
      if (Size != 0 && Count == Size)
      {
        RemoveItem(0);
        index--;
      }
      base.InsertItem(index, item);
    }
  }
}
