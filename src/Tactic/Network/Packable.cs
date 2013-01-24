using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Tactic.Network
{
  [DataContract(Namespace = Namespaces.JSON)]
  [KnownType("KnownTypes")]
  public class Packable
  {
    private static HashSet<Type> knownGameEvents = new HashSet<Type>() { };
    private static IEnumerable<Type> KnownEvents()
    {
      return knownGameEvents;
    }
    public static void AddKnownType(Type type)
    {
      knownGameEvents.Add(type);
    }
    public static void AddKnownType<T>()
    {
      AddKnownType(typeof(T));
    }
  }
}
