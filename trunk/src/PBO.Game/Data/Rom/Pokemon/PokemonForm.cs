using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Game
{
  [DataContract(Namespace=PBOMarks.PBO)]
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

    public PokemonSpecies Species
    { get; internal set; }

    [DataMember(EmitDefaultValue = false)]
    private readonly int _index;
    public int Index
    { get { return _index; } }

    [DataMember(EmitDefaultValue = false)]
    private readonly int _data;
    public PokemonFormData Data
    { get { return Species.GetData(_data); } }

    public BattleType Type1
    { get { return Species.Number == 493 && Index != 0 ? BattleTypeHelper.GetItemType(Index, 1) : Data.Type1; } }
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
