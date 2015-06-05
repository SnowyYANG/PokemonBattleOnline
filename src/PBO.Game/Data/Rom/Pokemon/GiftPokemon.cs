using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Game
{
  public class GiftPokemon
  {
    [DataMember]
#if EDITING
    public
#else
    private readonly
#endif
     short number;
    [DataMember(EmitDefaultValue = false)]
#if EDITING
    public
#else
    private readonly
#endif
     byte form;
    [DataMember]
#if EDITING
    public
#else
    private readonly
#endif
     short ownerId; //N精灵素大坑，日后再填
    [DataMember]
#if EDITING
    public
#else
    private readonly
#endif
     ushort personality; //0xffffffff for random
    [DataMember]
#if EDITING
    public
#else
    private readonly
#endif
     bool neverShiney; //闪光素大坑，日后再填

#if EDITING
    public
#else
    private
#endif
     GiftPokemon()
    {
    }

    [DataMember]
#if EDITING
    public
#else
    private readonly
#endif
     byte _gen;
    public int Gen
    { get { return _gen; } }
    
    [DataMember]
#if EDITING
    public
#else
    private readonly
#endif
     string _name;

    public PokemonForm Form
    { get { return RomData.GetPokemon(number, form); } }

    [DataMember]
#if EDITING
    public
#else
    private readonly
#endif
     PokemonGender _gender;
    public PokemonGender Gender
    { get { return _gender; } }

    [DataMember]
#if EDITING
    public
#else
    private readonly
#endif
     int _lv;
    public int Lv
    { get { return _lv; } }

    [DataMember]
#if EDITING
    public
#else
    private readonly
#endif
     int _ability; //不是AbilityIndex大丈夫？

    [DataMember(EmitDefaultValue = false)]
#if EDITING
    public
#else
    private readonly
#endif
     int _item;

    [DataMember(EmitDefaultValue = false)]
#if EDITING
    public
#else
    private readonly
#endif
     PokemonNature? _nature;

    [DataMember(EmitDefaultValue = false)]
#if EDITING
    public
#else
    private readonly
#endif
     ReadOnly6D _ivs; //0 for random

    [DataMember(EmitDefaultValue = false)]
#if EDITING
    public
#else
    private readonly
#endif
     int[] _moveIds;
    public IEnumerable<int> MoveIds
    { get { return _moveIds; } }

    [DataMember(EmitDefaultValue = false)]
#if EDITING
    public
#else
    private readonly
#endif
     bool _fatefulEncounter;
    public bool FatefulEncounter
    { get { return _fatefulEncounter; } }
  }
}
