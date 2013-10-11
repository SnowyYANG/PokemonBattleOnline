using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PokemonBattleOnline.Network
{
  public class Client : IPackReceivedListener, IDisposable
  {
    private static void OnKeepAlive(object state)
    {
      ((TcpClient)state).Sender.Send(PackHelper.EMPTYPACK);
    }

    public event Action Disconnected = delegate { };
    internal readonly TcpClient Network;
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
      State = new ClientState(this, login, cii);
      KeepAlive = new Timer(OnKeepAlive, network, PBOMarks.TIMEOUT, PBOMarks.TIMEOUT);
    }

    public IClientEventsListener Listener
    { internal get; set; }

    void IPackReceivedListener.OnPackReceived(byte[] pack)
    {
       Serializer.DeserializeFromCompressedJson<S2C>(pack).Execute(this);
    }
    public void SendC2S(C2S command)
    {
      Network.Sender.Send(Serializer.SerializeToCompressedJson(command));
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
