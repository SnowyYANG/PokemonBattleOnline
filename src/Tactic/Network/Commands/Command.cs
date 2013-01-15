using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Tactic.Network.Commands
{
  /// <summary>
  /// 区分ClientCommand和P2PCommand的最好办法：考虑后来登陆的客户端要获取的数据
  /// </summary>
  /// <typeparam name="TE"></typeparam>
  /// <typeparam name="TUE"></typeparam>
  [KnownType("KnownTypes")]
  [DataContract(Namespace = Namespaces.JSON)]
  public abstract class ClientCommand<TE, TUE>
  {
    static ClientCommand()
    {
      _types = new HashSet<Type>();
    }
    private static readonly HashSet<Type> _types;
    public static IEnumerable<Type> KnownTypes()
    {
      return _types;
    }
    public static void Register<T>()
    {
      _types.Add(typeof(T));
    }
    
    public abstract void Execute(Client<TE, TUE> client);
  }
  [KnownType("KnownTypes")]
  [DataContract(Namespace = Namespaces.JSON)]
  public abstract class P2PCommand<TE, TUE>
  {
    static P2PCommand()
    {
      _types = new HashSet<Type>();
    }
    private static readonly HashSet<Type> _types;
    public static IEnumerable<Type> KnownTypes()
    {
      return _types;
    }
    public static void Register<T>()
    {
      _types.Add(typeof(T));
    }
    
    public abstract void Execute(Client<TE, TUE> client, User<TUE> from);
  }
  [KnownType("KnownTypes")]
  [DataContract(Namespace = Namespaces.JSON)]
  public abstract class UserCommand<TE, TUE>
  {
    public abstract void Execute(ServerUser<TE, TUE> server);
  }
}
