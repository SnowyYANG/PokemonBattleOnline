using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using PokemonBattleOnline.Tactic.Network;
using PokemonBattleOnline.Network.Lobby;

namespace PokemonBattleOnline.Network
{
  /// <summary>
  /// 提供静态全局访问，事件同步到UI线程
  /// </summary>
  public static class PBOClient
  {
    public static event Action Disconnected;
    public static event Action CurrentChanged;
    public static event Action LoginFailed_Full;
    public static event Action LoginFailed_BadName;
    public static event Action LoginFailed_BadVersion;
    private static readonly object Locker;
    private static LoginClient currentLogin;

    static PBOClient()
    {
      Locker = new object();
    }

    private static Client _current;
    public static Client Current
    {
      get
      {
        lock (Locker)
        {
          return _current;
        }
      }
      private set
      {
        lock (Locker)
        {
          if (_current == null)
          {
            _current = value;
            if (value != null) UIDispatcher.Invoke(CurrentChanged);
          }
        }
      }
    }

    private static void LoginSucceed(Client obj)
    {
      Current = obj;
    }

    public static void Login(IPAddress serverIp, int serverPort, string name, ushort avatar)
    {
      lock (Locker)
      {
        if (currentLogin == null && _current == null)
        {
          currentLogin = new LoginClient(Factory.TryTcpConnect(serverIp, serverPort));
          currentLogin.BadName += LoginFailed_BadName;
          currentLogin.LoginSucceed += LoginSucceed;
          currentLogin.BeginLogin(name, avatar);
        }
      }
    }
    public static void Login(string server, string name, ushort avatar)
    {
      if (Current == null && currentLogin == null)
      {
        System.Net.IPAddress ip;
        if (!System.Net.IPAddress.TryParse(server, out ip))
          try
          {
            var ips = System.Net.Dns.GetHostAddresses(server);
            foreach (var i in ips)
              if (i.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
              {
                ip = i;
                break;
              }
          }
          catch { }
        if (ip != null) Login(ip, PBOMarks.DEFAULT_PORT, name, avatar);
      }
    }

    public static void Dispose()
    {
      Current.Dispose();
      Current = null;
    }
  }
}
