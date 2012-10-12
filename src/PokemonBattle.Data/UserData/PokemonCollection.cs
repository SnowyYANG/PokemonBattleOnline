using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace LightStudio.PokemonBattle.Data
{
	[CollectionDataContract(Namespace = Namespaces.PBO, ItemName="PokemonData")]
  public abstract class PokemonCollection : ObservableCollection<PokemonData>
	{
    internal PokemonCollection()
    {
    }

    public abstract string Name
    { get; set; }

    public int Size
    { get; internal set; }

    public abstract bool CanAdd
    { get; }

    protected override void SetItem(int index, PokemonData item)
    {
      item.Container = this;
      base.SetItem(index, item);
    }
    protected override void InsertItem(int index, PokemonData item)
    {
      item.Container = this;
      base.InsertItem(index, item);
    }
	}
}
