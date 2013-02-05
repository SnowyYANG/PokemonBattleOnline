using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using PokemonBattleOnline.Tactic.Network;

namespace PokemonBattleOnline.Network.Lobby
{
  public class ServerUser : UserBase
  {
    private readonly Server Server;

    internal ServerUser(LoginUser user, Server server)
      : base(user.Network)
    {
      Network.Disconnect += RemoveUser;
      _user = new User(user.Network.Id, user.Name, user.Avatar);
      Server = server;
      LastPack = DateTime.Now;
    }

    private readonly User _user;
    public User User
    { get { return _user; } }
    internal DateTime LastPack
    { get; private set; }

    private void RemoveUser()
    {
      Server.RemoveUser(this);
      LastPack = DateTime.Now;
    }
    protected override void OnPackReceived(byte[] pack)
    {
      if (!pack.IsEmpty())
      {
        var h = pack.GetHeader();
        var c = pack.GetContent();
        switch (h)
        {
          case 0:
            Serializer.DeserializeFromJson<UserCommand>(c).Execute(this);
            break;
          case 1: //p2p
            {
              var n = c[0];
              var receivers = new int[n]; //n == 0 means all
              var offset = 1;
              for (int i = 0; i < n; ++i)
              {
                receivers[i] = c.SubArray(offset, 2).ToInt16().Value;
                offset += 2;
              }
              Server.SendP2PPack(this, c.SubArray(offset), receivers);
            }
            break;
          case 10: //群聊专用
            try
            {
              Server.PublicChat(this, Encoding.Unicode.GetString(pack));
            }
            catch
            {
              goto default;
            }
            break;
          default:
            //超过一定额度 close
            break;
        }
      }
    }
  }
}
