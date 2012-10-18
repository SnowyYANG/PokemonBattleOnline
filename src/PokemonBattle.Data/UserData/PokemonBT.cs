using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.ComponentModel;
using System.Runtime.Serialization;
using LightStudio.Tactic;

namespace LightStudio.PokemonBattle.Data
{
  [CollectionDataContract(Namespace = Namespaces.PBO, ItemName="PokemonData")]
  public class PokemonBT : PokemonCollection, INotifyPropertyChanged
  {
    private static readonly PropertyChangedEventArgs NAME = new PropertyChangedEventArgs("Name");

    internal static event Action<PokemonData> PokemonDeleted;

    internal PokemonBT()
      : base()
    {
    }

    private string _name;
    public override string Name
    {
      get { return _name; }
      set
      {
        if (_name != value)
        {
          _name = value;
          OnPropertyChanged(NAME);
        }
      }
    }
    public override bool CanAdd
    { get { return Count != Size; } }
    
    protected override void InsertItem(int index, PokemonData item)
    {
      if (Size == 0 || Count < Size) base.InsertItem(index, item);
    }
    protected override void RemoveItem(int index)
    {
      var p = this[index];
      if (p.Container == this) PokemonDeleted(p);
      base.RemoveItem(index);
    }
    protected override void ClearItems()
    {
      foreach (var pm in this) PokemonDeleted(pm);
      base.ClearItems();
    }
    public void Export(Stream stream)
    {
      Serializer.Serialize(this, stream);
    }
  }
}
