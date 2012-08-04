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
  
  [DataContract(Namespace=Namespaces.LIGHT)]
  public class Item : GameElement
  {
    [DataMember(EmitDefaultValue = false)]
    public ItemType Type { get; private set; }

    [DataMember]
    public int FlingPower { get; private set; }

    //public IExecutableElement<IController> FlingEffect { get; internal set; }
    
    public Item(int id)
      : base(id)
    {
    }
  }
}
