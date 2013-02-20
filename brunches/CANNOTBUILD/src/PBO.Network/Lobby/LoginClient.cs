using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using PokemonBattleOnline.Tactic.Network;

namespace PokemonBattleOnline.Network.Lobby
{
  internal class LoginClient : IPackReceivedListener
  {
    private static void OnLoginTimeout(object state)
    {
      var c = (LoginClient)state;
      c.OnLoginFailed(c.Disconnected);
    }
    
    public event Action<Client> LoginSucceed;
    public event Action Disconnected;
    public event Action BadVersion;
    public event Action BadName;
    public event Action Full;

    private readonly string Server;
    private readonly int Port;
    private readonly string Name;
    private readonly ushort Avatar;
    private readonly Timer TimeBomb;

    public LoginClient(string server, int port, string name, ushort avatar)
    {
      Server = server;
      Port = port;
      Name = name;
      Avatar = avatar;
      TimeBomb = new Timer(OnLoginTimeout, this, Timeout.Infinite, Timeout.Infinite);
    }

    public void BeginLogin()
    {
      System.Net.IPAddress ip;
      if (System.Net.IPAddress.TryParse(Server, out ip)) ClientFactory.TryTcpConnect(ip, Port, TcpConnectCallback);
      else ClientFactory.TryTcpConnect(Server, Port, TcpConnectCallback);
    }
    
    private INetworkClient Network;
    private void TcpConnectCallback(INetworkClient client)
    {
      if (client == null) OnLoginFailed(Disconnected);
      else
      {
        TimeBomb.Change(30000, Timeout.Infinite);
        Network = client;
        Network.Listener = this;
        Network.Send(PBOMarks.VERSION);
      }
    }
    private void OnLoginFailed(Action raiseEvent)
    {
      TimeBomb.Dispose();
      raiseEvent();
      if (Network != null) Network.Dispose();
    }

    private byte state;
    void IPackReceivedListener.OnPackReceived(byte[] pack)
    {
      switch (state)
      {
        case 0: //version，失败直接结束，满员
          if (pack.IsEmpty())
          {
            state++;
            Network.Send(Name);
          }
          else if (pack.ToByte() == 'f') OnLoginFailed(Full); //实际上这条消息在version发送前就会收到
          else OnLoginFailed(BadVersion);
          break;
        case 1: //Name，失败可重试，错误直接结束
          if (pack.IsEmpty())
          {
            state = 2;
            Network.Send(Avatar);
          }
          else OnLoginFailed(BadName);
          break;
        case 2: //ClientStartUpInfo
          var init = pack.ToObject<ClientInitInfo>();
          if (init == null) OnLoginFailed(Disconnected);
          else
          {
            TimeBomb.Dispose();
            LoginSucceed(new Client(Network, init));
          }
          break;
      }
    }
  }
}
