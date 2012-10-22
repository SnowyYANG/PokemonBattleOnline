using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.Tactic.DataModels;

namespace LightStudio.PokemonBattle.Data
{
  [DataContract(Namespace=Namespaces.PBO)]
  public class PokemonForm
  {
#if DEBUG
    public PokemonForm()
    {
    }
#else
    private PokemonForm()
    {
    }
#endif

    public PokemonType Type
    { get; internal set; }

    [DataMember(EmitDefaultValue = false)]
    private readonly byte _index;
    public int Index
    { get { return _index; } }

    [DataMember(EmitDefaultValue = false)]
    private readonly string _name;
    public string Name
    { get { return _name; } }

    [DataMember(EmitDefaultValue = false)]
    private readonly byte _data;
    public PokemonFormData Data
    { get { return Type.GetData(_data); } }

    public BattleType Type1
    { get { return Type.Number == 493 && Index != 0 ? BattleTypeHelper.GetItemType(Index, 1) : Data.Type1; } }
    public BattleType Type2
    { 
      get
      { 
        var data = Data;
        return data.Type1 == data.Type2 ? BattleType.Invalid : data.Type2;
      }
    }
  }
}
