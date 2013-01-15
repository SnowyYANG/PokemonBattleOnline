using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Tactic.Network
{
  internal class LoginUser<T> : UserBase
  {
    private readonly Action<LoginUser<T>> LoginSucceeded;
    private byte versionHeader;
    private int version;
    
    public LoginUser(INetworkUser network, Action<LoginUser<T>> onSucceeded)
      : base(network)
    {
      LoginSucceeded = onSucceeded;
    }

    public string Name
    { get; private set; }
    public T Extension
    { get; private set; }

    private int state;
    protected override void OnPackReceived(byte[] pack)
    {
      switch (state)
      {
        case 0: //version，失败直接结束

          break;
        case 1: //Name，失败可重试，错误直接结束

          break;
        case 2: //T，错误直接结束
          LoginSucceeded(this);
          break;
      }
    }
  }
}
