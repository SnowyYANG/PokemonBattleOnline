using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Network.Lobby;

namespace PokemonBattleOnline.Network
{
  /// <summary>
  /// 提供静态全局访问
  /// </summary>
  public static class PBOServer
  {
    private static readonly object Locker;

    static PBOServer()
    {
      Locker = new object();
    }

    private static Server _current;
    public static Server Current
    {
      get
      {
        lock (Locker)
        {
          return _current;
        }
      }
      set
      {
        lock (Locker)
        {
          _current = value;
        }
      }
    }
    
    public static void NewTcpServer(int port)
    {
      Current = new Server(Tactic.Network.Factory.NewTcpServer(port));
    }
  }
}
