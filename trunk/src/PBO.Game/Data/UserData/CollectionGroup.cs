using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Game
{
  [CollectionDataContract(Namespace = PBOMarks.PBO, ItemName="PokemonBT")]
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
    public bool Import(string name, string source)
    {
      PokemonBT collection = Helper.Import(source, CollectionSize);
      //TODO: validate
      if (collection != null)
      {
        collection.Name = name;
        Add(collection);
        return true;
      }
      return false;
    }
  }
}
