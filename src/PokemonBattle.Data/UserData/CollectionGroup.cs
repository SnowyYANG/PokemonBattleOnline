using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using LightStudio.Tactic;

namespace LightStudio.PokemonBattle.Data
{
  [CollectionDataContract(Namespace = Namespaces.PBO, ItemName="PokemonBT")]
  public class CollectionGroup : ObservableCollection<PokemonBT>
  {
    internal CollectionGroup()
    {
    }

    private int _collectionSize;
    public int CollectionSize
    {
      get { return _collectionSize; }
      internal set
      {
        _collectionSize = value;
        foreach (var bt in this) bt.Size = CollectionSize;
      }
    }

    protected override void InsertItem(int index, PokemonBT item)
    {
      item.Size = CollectionSize;
      base.InsertItem(index, item);
    }
    protected override void SetItem(int index, PokemonBT item)
    {
      item.Size = CollectionSize;
      base.SetItem(index, item);
    }
    protected override void RemoveItem(int index)
    {
      this[index].Clear();
      base.RemoveItem(index);
    }

    public void AddCollection(string name)
    {
      Add(new PokemonBT() { Name = name });
    }
    public void Import(string name, System.IO.Stream stream)
    {
      PokemonBT collection = Serializer.Deserialize<PokemonBT>(stream);
      //TODO: validate
      collection.Name = name;
      if (collection != null) Add(collection);
    }
  }
}
