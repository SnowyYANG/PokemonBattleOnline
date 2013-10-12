using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;
using System.Runtime.Serialization.Json;
using PokemonBattleOnline.Network.C2Ss;

namespace PokemonBattleOnline.Network
{
  public class Server : IDisposable
  {
    private static readonly DataContractJsonSerializer C2SSerializer;
    private static readonly DataContractJsonSerializer S2CSerializer;
    static Server()
    {
      C2SSerializer = new DataContractJsonSerializer(typeof(IC2S), new Type[] { typeof(ChatC2S), typeof(SetSeat) });
      S2CSerializer = new DataContractJsonSerializer(typeof(S2C));
    }

    public event Action<User> UsersUpdate;
    
    internal readonly TcpServer Network;
    internal readonly object UserLocker;
    private readonly LoginServer Login;
    public readonly ServerState State;
    private readonly ConcurrentDictionary<int, ServerUser> users;

    internal Server(int port)
    {
      Network = new TcpServer(port);
      UserLocker = new object();
      Login = new LoginServer(Network, this);
      users = new ConcurrentDictionary<int, ServerUser>();
    }

    public void Start()
    {
      Network.IsListening = true;
    }

    public void Dispose()
    {
      Network.Dispose();
      Login.Dispose();
    }
  }
}
