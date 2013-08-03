using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.Tactic.DataModels;

namespace LightStudio.PokemonBattle.Data
{
  public enum ItemType
  {
    Normal,
    OneTime,
    Berry
  }
  
  [DataContract(Namespace=PBOMarks.PBO)]
  public class Item : GameElement
  {
    [DataMember(EmitDefaultValue = false)]
    public ItemType Type { get; private set; }

    [DataMember]
    public int FlingPower { get; private set; }

#if EDITING
    public
#else
    private 
#endif
     Item(int id)
      : base(id)
    {
    }

    public override string Name
    { get { return DataService.DataString[EnglishName]; } }
    public override string Description
    { get { return null; } }
  }
}
