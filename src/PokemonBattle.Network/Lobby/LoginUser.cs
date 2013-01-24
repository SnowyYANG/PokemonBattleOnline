using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Tactic.Network;

namespace PokemonBattleOnline.Network.Lobby
{
  internal class LoginUser : UserBase
  {
    private readonly LoginServer Server;
    
    public LoginUser(INetworkUser network, LoginServer server)
      : base(network)
    {
      Server = server;
    }

    public string Name
    { get; private set; }
    public ushort Avatar
    { get; private set; }

    private byte state;
    protected override void OnPackReceived(byte[] pack)
    {
      switch (state)
      {
        case 0: //version，失败直接结束
          if (pack.ToInt16() == null) OnLoginFailed(LoginFailedReason.BadVersion);
          else
          {
            state = 1;
            Network.SendEmpty();
          }
          break;
        case 1: //Name，失败可重试，错误直接结束
          var name = pack.ToUnicodeString();
          if (Server.RegisterUserName(this, name))
          {
            state = 2;
            Name = name;
            Network.SendEmpty();
          }
          else OnLoginFailed(LoginFailedReason.BadName);
          break;
        case 2: //Avatar，错误直接结束
          var av = pack.ToInt16();
          if (av.HasValue)
          {
            Avatar = av.Value;
            Server.LoginComplete(this);
          }
          else OnBadPack();
          break;
      }
    }
    private void OnLoginFailed(LoginFailedReason reason)
    {
    }

    public override void Dispose()
    {
      Server.BadLogin(this);
      base.Dispose();
    }
  }
}
