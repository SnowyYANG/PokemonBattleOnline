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
    public static event Action LoginFailed_Name;
    public static event Action LoginFailed_Version;
    private static readonly object Locker;

    static PBOClient()
    {
      Locker = new object();
    }

    private static Client _current;
    /// <summary>
    /// get is not thread safe
    /// </summary>
    public static Client Current
    {
      get { return _current; }
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

    private static LoginClient currentLogin;
    public static bool Login(string server, string name, ushort avatar)
    {
      return Login(server, PBOMarks.DEFAULT_PORT, name, avatar);
    }
    public static bool Login(string server, int port, string name, ushort avatar)
    {
      lock (Locker)
      {
        if (_current == null && currentLogin == null)
        {
          currentLogin = new LoginClient(server, port, name, avatar);
          currentLogin.Disconnected += OnDisconnected;
          currentLogin.BadVersion += OnLoginFailed_Version;
          currentLogin.BadName += OnLoginFailed_Name;
          currentLogin.Full += OnLoginFailed_Full;
          currentLogin.BeginLogin();
          return true;
        }
        return false;
      }
    }

    private static void OnLoginFailed_Full()
    {
      UIDispatcher.BeginInvoke(LoginFailed_Full);
    }
    private static void OnLoginFailed_Name()
    {
      UIDispatcher.BeginInvoke(LoginFailed_Name);
    }
    private static void OnLoginFailed_Version()
    {
      UIDispatcher.BeginInvoke(LoginFailed_Version);
    }
    private static void OnDisconnected()
    {
      UIDispatcher.BeginInvoke(Disconnected);
    }

    public static void Dispose()
    {
      Current.Dispose();
      Current = null;
    }
  }
}
