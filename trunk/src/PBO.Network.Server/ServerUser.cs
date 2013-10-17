using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Runtime.Serialization.Json;
using PokemonBattleOnline.Network.C2Ss;

namespace PokemonBattleOnline.Network
{
  internal class ServerUser : UserBase
  {
    private static readonly DataContractJsonSerializer C2SSerializer;
    private static readonly DataContractJsonSerializer S2CSerializer;
    static ServerUser()
    {
      C2SSerializer = new DataContractJsonSerializer(typeof(IC2S), new Type[] { typeof(ChatC2S), typeof(SetSeatC2S) });
      S2CSerializer = new DataContractJsonSerializer(typeof(S2C));
    }

    public readonly Server Server;

    public ServerUser(LoginUser user, Server server)
      : base(user.Network)
    {
      Network.Disconnected += Dispose;
      _user = new User(user.Network.Id, user.Name, user.Avatar);
      Server = server;
      LastPack = DateTime.Now;
    }

    private readonly User _user;
    public User User
    { get { return _user; } }
    internal DateTime LastPack
    { get; private set; }

    protected override void OnPackReceived(byte[] pack)
    {
      LastPack = DateTime.Now;
      try
      {
        if (!pack.IsEmpty())
          using (var ms = new MemoryStream(pack, false))
          using (var ds = new DeflateStream(ms, CompressionMode.Decompress))
            ((IC2S)C2SSerializer.ReadObject(ds)).Execute(this);
      }
      catch
      {
        Dispose();
      }
    }
    public void Send(S2C s2c)
    {
      using (var ms = new MemoryStream())
      {
        using (var ds = new DeflateStream(ms, CompressionMode.Compress))
          S2CSerializer.WriteObject(ds, s2c);
        Network.Sender.Send(ms.ToArray());
      }
    }

    public override void Dispose()
    {
      Server.RemoveUser(this);
      base.Dispose();
    }
  }
}
