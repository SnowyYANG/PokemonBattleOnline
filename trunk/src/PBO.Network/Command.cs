using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using PokemonBattleOnline.Network.Commands;

namespace PokemonBattleOnline.Network
{
  [KnownType("KnownTypes")]
  [DataContract(Namespace = PBOMarks.JSON)]
  public abstract class S2C
  {
    static Type[] knownTypes = new Type[] { typeof(ChatS2C), typeof(SetUserSeat), typeof(UserChanged) };
    static IEnumerable<Type> KnownTypes()
    {
      return knownTypes;
    }
    
    public abstract void Execute(Client client);
  }

  public interface IC2S
  {
  }
}
