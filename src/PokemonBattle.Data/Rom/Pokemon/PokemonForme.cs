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
  public class PokemonForme
  {
#if DEBUG
    public PokemonForme()
    {
    }
#endif

    public PokemonType Type
    { get; internal set; }

    [DataMember(EmitDefaultValue = false)]
    private byte _index;
    public int Index
    { 
      get { return _index; }
      private set { _index = (byte)value; }
    }

    [DataMember(EmitDefaultValue = false)]
    private byte _data;
    public PokemonFormeData Data
    { get { return Type.GetData(_data); } }
  }
}
