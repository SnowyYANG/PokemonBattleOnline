using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LightStudio.PokemonBattle.Data
{
  public class GiftPokemon
  {
    [DataMember]
    public short number;
    [DataMember(EmitDefaultValue = false)]
    public byte form;
    [DataMember]
    public short ownerId; //N精灵素大坑，日后再填
    [DataMember]
    public ushort personality; //0xffffffff for random
    [DataMember]
    public bool neverShiney; //闪光素大坑，日后再填

#if DEBUG
    public GiftPokemon()
    {
    }
#else
    private GiftPokemon()
    {
    }
#endif

    private byte _gen;
    public int Gen
    { get { return _gen; } }
    
    [DataMember]
    public string _name;

    public PokemonForm Form
    { get { return GameDataService.GetPokemon(number, form); } }

    [DataMember]
    public PokemonGender _gender;
    public PokemonGender Gender
    { get { return _gender; } }

    [DataMember]
    public int _lv;
    public int Lv
    { get { return _lv; } }

    [DataMember]
    public int _ability; //不是AbilityIndex大丈夫？

    [DataMember(EmitDefaultValue = false)]
    public int _item;

    [DataMember(EmitDefaultValue = false)]
    public PokemonNature? _nature;

    [DataMember(EmitDefaultValue = false)]
    public ReadOnly6D _ivs; //0 for random

    [DataMember(EmitDefaultValue = false)]
    public int[] _moveIds;
    public IEnumerable<int> MoveIds
    { get { return _moveIds; } }

    [DataMember(EmitDefaultValue = false)]
    public bool _fatefulEncounter;
    public bool FatefulEncounter
    { get { return _fatefulEncounter; } }
  }
}
