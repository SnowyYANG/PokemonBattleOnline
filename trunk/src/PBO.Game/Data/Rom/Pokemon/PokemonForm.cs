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
#if EDITING
    public
#else
    private readonly
#endif
     int _index;
    public int Index
    { get { return _index; } }

    [DataMember(EmitDefaultValue = false)]
#if EDITING
    public
#else
    private readonly
#endif
     int _data;
    public PokemonFormData Data
    { get { return Species.GetData(_data); } }

    private static readonly BattleType[] ARCEUS = new BattleType[] { BattleType.Normal, BattleType.Fire, BattleType.Water, BattleType.Electric, BattleType.Grass, BattleType.Ice, BattleType.Fighting, BattleType.Poison, BattleType.Ground, BattleType.Flying, BattleType.Psychic, BattleType.Bug, BattleType.Rock, BattleType.Ghost, BattleType.Dragon, BattleType.Dark, BattleType.Steel, BattleType.Fairy };
    public BattleType Type1
    { get { return Species.Number == 493 ? ARCEUS[_index] : Data.Type1; } }
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
