using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace PokemonBattleOnline.Network
{
  /// <summary>
  /// 提供静态全局访问
  /// </summary>
  public static class PBOClient
  {
    public static event Action Disconnected
    {
      add { ClientController.Disconnected += value; }
      remove { ClientController.Disconnected -= value; }
    }
    public static event Action CurrentChanged;
    public static event Action LoginFailed_Full
    {
      add { LoginClient.Full += value; }
      remove { LoginClient.Full -= value; }
    }
    public static event Action LoginFailed_Name
    {
      add { LoginClient.BadName += value; }
      remove { LoginClient.BadName -= value; }
    }
    public static event Action LoginFailed_Version
    {
      add { LoginClient.BadVersion += value; }
      remove { LoginClient.BadVersion -= value; }
    }
    public static event Action LoginFailed_Disconnect
    {
      add { LoginClient.Disconnect += value; }
      remove { LoginClient.Disconnect -= value; }
    }
    private static readonly object Locker = new object();

    static PBOClient()
    {
      LoginClient.LoginSucceed += LoginSucceed;
      LoginClient.BadName += LoginFailed;
      LoginClient.BadVersion += LoginFailed;
      LoginClient.Disconnect += LoginFailed;
      LoginClient.Full += LoginFailed;
      ClientController.Disconnected += DisposeCurrent;
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
          _current = value;
          UIDispatcher.Invoke(CurrentChanged);
        }
      }
    }

    private static void LoginSucceed(Client obj)
    {
      currentLogin = null;
      Current = obj;
    }
    private static void LoginFailed()
    {
      currentLogin = null;
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
          currentLogin.BeginLogin();
          return true;
        }
        return false;
      }
    }

    public static void DisposeCurrent()
    {
      lock (Locker)
      {
        if (Current != null)
        {
          Current.Dispose();
          Current = null;
        }
      }
    }
  }
}
