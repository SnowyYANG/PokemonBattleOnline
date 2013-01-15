using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Tactic.Network
{
  public enum LoginFailedReason
  {
    BadVersion,
    BadName,
    Full,
    TimeOut //服务器无回音
  }
  internal class LoginClient<TE, TUE> : ClientBase
  {
    public event Action<Client<TE, TUE>> LoginSucceed;
    public event Action<LoginFailedReason, bool> LoginFailed;
    private bool logging;
    private TUE extension;

    public LoginClient(INetworkClient network)
      : base(network)
    {
    }

    private void OnLoginFailed(LoginFailedReason reason, bool dispose = true)
    {
      logging = false;
      LoginFailed(reason, dispose);
      if (dispose) Network.Dispose();
    }

    private string name;
    public void BeginLogin(string name, TUE extension)
    {
      lock (this)
      {
        if (!logging)
        {
          logging = true;
          this.name = name;
          this.extension = extension;
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
          else if (pack.ToByte() == 'f') OnLoginFailed(LoginFailedReason.Full); //实际上这条消息在version发送前就会收到
          else OnLoginFailed(LoginFailedReason.BadVersion);
          break;
        case 1: //Name，失败可重试，错误直接结束
          if (pack.IsEmpty())
          {
            state++;
            Network.Send(extension);
          }
          else OnLoginFailed(LoginFailedReason.BadName, false);
          break;
        case 2: //ClientStartUpInfo
          var init = pack.ToObject<ClientInitInfo<TE, TUE>>();
          if (init == null) OnBadPack();
          else LoginSucceed(new Client<TE, TUE>(Network, init));
          break;
      }
    }
  }
}
