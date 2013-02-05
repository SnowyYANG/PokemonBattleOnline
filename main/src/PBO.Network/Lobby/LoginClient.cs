using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Tactic.Network;

namespace PokemonBattleOnline.Network.Lobby
{
  internal class LoginClient : ClientBase
  {
    public event Action<Client> LoginSucceed;
    public event Action BadVersion;
    public event Action BadName;
    public event Action Full;
    private bool logging;

    public LoginClient(INetworkClient network)
      : base(network)
    {
    }

    private void OnLoginFailed(Action raiseEvent)
    {
      logging = false;
      raiseEvent();
      Dispose();
    }

    private string name;
    private ushort avatar;
    public void BeginLogin(string name, ushort avatar)
    {
      lock (this)
      {
        if (!logging)
        {
          logging = true;
          this.name = name;
          this.avatar = avatar;
          Network.Send((ushort)0);
        }
      }
    }
    private int state;
    protected override void OnPackReceived(byte[] pack)
    {
      switch (state)
      {
        case 0: //version，失败直接结束，满员
          if (pack.IsEmpty())
          {
            state++;
            Network.Send(name);
          }
          else if (pack.ToByte() == 'f') OnLoginFailed(Full); //实际上这条消息在version发送前就会收到
          else OnLoginFailed(BadVersion);
          break;
        case 1: //Name，失败可重试，错误直接结束
          if (pack.IsEmpty())
          {
            state = 2;
            Network.Send(avatar);
          }
          else OnLoginFailed(BadName);
          break;
        case 2: //ClientStartUpInfo
          var init = pack.ToObject<ClientInitInfo>();
          if (init == null) OnBadPack();
          else LoginSucceed(new Client(Network, init));
          break;
      }
    }
  }
}
