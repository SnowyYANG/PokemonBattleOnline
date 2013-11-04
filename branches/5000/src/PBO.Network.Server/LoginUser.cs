﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PokemonBattleOnline.Network
{
  internal class LoginUser : UserBase
  {
    private readonly LoginServer Server;
    
    public LoginUser(TcpUser network, LoginServer server)
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
      //当前版本失败一律直接结束，不给重试
      switch (state)
      {
        case 0: //version
          if (pack.ToUInt16() == null) Dispose();
          else
          {
            state = 1;
            Network.Sender.SendEmpty();
          }
          break;
        case 1: //Name
          var name = pack.ToUnicodeString();
          if (Server.RegisterName(this, name))
          {
            state = 2;
            Name = name;
            Network.Sender.SendEmpty();
          }
          else Dispose();
          break;
        case 2: //Avatar
          var av = pack.ToUInt16();
          if (av.HasValue)
          {
            Avatar = av.Value;
            Server.LoginComplete(this);
          }
          else Dispose();
          break;
      }
    }

    public override void Dispose()
    {
      Server.BadLogin(this);
      base.Dispose();
    }
  }
}