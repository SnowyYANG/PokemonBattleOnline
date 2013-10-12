using System;
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
      C2SSerializer = new DataContractJsonSerializer(typeof(IC2S), new Type[] { typeof(ChatC2S), typeof(SetSeat) });
      S2CSerializer = new DataContractJsonSerializer(typeof(S2C));
    }

    private static void OnKeepAlive(object state)
    {
      ((TcpClient)state).Sender.Send(PackHelper.EMPTYPACK);
    }

    public event Action Disconnected = delegate { };
    internal readonly TcpClient Network;
    public readonly ClientController Controller;
    internal readonly ClientState State;
    private readonly Timer KeepAlive;
    
    internal Client(TcpClient network, LoginClient login, ClientInitInfo cii)
    {
      Network = network;
      network.Listener = this;
      network.Disconnect += () =>
        {
          if (!isDisposed)
          {
            Disconnected();
            Dispose();
          }
        };
      Controller = new ClientController(this);
      State = new ClientState(this, login, cii);
      KeepAlive = new Timer(OnKeepAlive, network, PBOMarks.TIMEOUT, PBOMarks.TIMEOUT);
    }

    public IClientEventsListener Listener
    { internal get; set; }

    void IPackReceivedListener.OnPackReceived(byte[] pack)
    {
      using (var ms = new MemoryStream(pack, false))
      using (var ds = new DeflateStream(ms, CompressionMode.Decompress))
      {
        var s2c = (S2C)S2CSerializer.ReadObject(ds);
        s2c.Execute(this);
      }
    }
    public void SendC2S(IC2S command)
    {
      using (var ms = new MemoryStream())
      using (var ds = new DeflateStream(ms, CompressionMode.Compress))
      {
        C2SSerializer.WriteObject(ds, command);
        Network.Sender.Send(ms.ToArray());
      }
    }

    private void SendKeepAlive()
    {
      Network.Sender.SendEmpty();
    }

    private volatile bool isDisposed;
    public void Dispose()
    {
      if (!isDisposed)
      {
        isDisposed = true;
        Network.Dispose();
      }
    }
  }
}
