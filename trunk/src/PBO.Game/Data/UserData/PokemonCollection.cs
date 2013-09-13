using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Game
{
	[CollectionDataContract(Namespace = PBOMarks.PBO, ItemName="PokemonData")]
  public abstract class PokemonCollection : ObservableCollection<PokemonData>
	{
    internal PokemonCollection()
    {
    }
    internal PokemonCollection(IEnumerable<PokemonData> pms)
      : base(pms)
    {
    }

    public abstract string Name
    { get; set; }

    private int _size;
    public int Size
    {
      get { return _size; }
      internal set
      {
        if (_size != value)
        {
          _size = value;
          var removes = Count - _size;
          while (--removes >= 0) RemoveAt(_size);
        }
      }
    }

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
