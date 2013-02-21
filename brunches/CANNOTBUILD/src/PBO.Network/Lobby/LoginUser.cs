﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using PokemonBattleOnline.Tactic.Network;

namespace PokemonBattleOnline.Network.Lobby
{
  internal class LoginUser : UserBase
  {
    private static void OnLoginTimeout(object state)
    {
      ((LoginUser)state).OnLoginFailed();
    }

    private readonly LoginServer Server;
    private readonly Timer TimeBomb;
    
    public LoginUser(INetworkUser network, LoginServer server)
      : base(network)
    {
      Server = server;
      TimeBomb = new Timer(OnLoginTimeout, this, PBOMarks.TIMEOUT, Timeout.Infinite);
      network.Disconnect += OnLoginFailed;
    }

    public string Name
    { get; private set; }
    public ushort Avatar
    { get; private set; }

    private byte state;
    protected override void OnPackReceived(byte[] pack)
    {
      //当前版本失败一律直接结束，不给重试
      switch (state)
      {
        case 0: //version
          if (pack.ToUInt16() == null) OnLoginFailed();
          else
          {
            state = 1;
            Network.SendEmpty();
          }
          break;
        case 1: //Name
          var name = pack.ToUnicodeString();
          if (Server.RegisterUserName(this, name))
          {
            state = 2;
            Name = name;
            Network.SendEmpty();
          }
          else OnLoginFailed();
          break;
        case 2: //Avatar
          var av = pack.ToUInt16();
          if (av.HasValue)
          {
            Avatar = av.Value;
            Server.LoginComplete(this);
          }
          else OnLoginFailed();
          break;
      }
    }
    private void OnLoginFailed()
    {
      Dispose();
    }

    public override void Dispose()
    {
      TimeBomb.Dispose();
      Server.BadLogin(this);
      base.Dispose();
    }
  }
}
