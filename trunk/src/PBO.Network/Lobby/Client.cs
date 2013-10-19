﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Runtime.Serialization.Json;
using PokemonBattleOnline.Network.Commands;

namespace PokemonBattleOnline.Network
{
  public class Client : IPackReceivedListener, IDisposable
  {
    private static readonly DataContractJsonSerializer C2SSerializer;
    private static readonly DataContractJsonSerializer S2CSerializer;

    static Client()
    {
      var c2s = typeof(IC2S);
      C2SSerializer = new DataContractJsonSerializer(c2s, c2s.SubClasses().ToArray());
      var s2c = typeof(IS2C);
      S2CSerializer = new DataContractJsonSerializer(s2c, s2c.SubClasses().ToArray());
    }

    private static void OnKeepAlive(object state)
    {
      ((TcpClient)state).Sender.SendEmpty();
    }

    public event Action Disconnected = delegate { };
    internal readonly TcpClient Network;
    public readonly ClientController Controller;
    public readonly ClientState State;
    private readonly Timer KeepAlive;
    
    internal Client(TcpClient network, LoginClient login, ClientInitInfo cii)
    {
      Network = network;
      network.Listener = this;
      network.Disconnected += () =>
        {
          if (!isDisposed)
          {
            Dispose();
            Disconnected();
          }
        };
      Controller = new ClientController(this);
      State = new ClientState(this, login, cii);
      KeepAlive = new Timer(OnKeepAlive, network, PBOMarks.TIMEOUT, PBOMarks.TIMEOUT);
    }

    void IPackReceivedListener.OnPackReceived(byte[] pack)
    {
      if (!pack.IsEmpty())
        using (var ms = new MemoryStream(pack, false))
        using (var ds = new DeflateStream(ms, CompressionMode.Decompress))
        {
          var s2c = (IS2C)S2CSerializer.ReadObject(ds);
          UIDispatcher.Invoke((Action<Client>)s2c.Execute, this);
        }
    }
    public void SendC2S(IC2S command)
    {
      using (var ms = new MemoryStream())
      {
        using (var ds = new DeflateStream(ms, CompressionMode.Compress, true))
          C2SSerializer.WriteObject(ds, command);
        Network.Sender.Send(ms.ToArray());
      }
    }

    private volatile bool isDisposed;
    public void Dispose()
    {
      if (!isDisposed)
      {
        KeepAlive.Dispose();
        Network.Dispose();
        isDisposed = true;
      }
    }
  }
}
