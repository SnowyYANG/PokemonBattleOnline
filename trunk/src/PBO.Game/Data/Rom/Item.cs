using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Game
{
  public enum ItemType
  {
    Normal,
    OneTime,
    Berry
  }
  
  [DataContract(Namespace=PBOMarks.PBO)]
  public class Item
  {
    [DataMember]
    public int Id;
    
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
    {
      Id = id;
    }
  }
}
