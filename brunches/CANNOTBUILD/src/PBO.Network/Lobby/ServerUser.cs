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
        switch (pack[0])
        {
          case 0:
            Serializer.DeserializeFromJson<UserCommand>(pack, 1).Execute(this);
            break;
          case 1: //p2p
            {
              var n = pack[1];
              var receivers = new int[n]; //n == 0 means all
              var offset = 2;
              for (int i = 0; i < n; ++i)
              {
                receivers[i] = pack.ToUInt16(offset).Value;
                offset += 2;
              }
              Server.SendP2PPack(this, pack.SubArray(offset), receivers);
            }
            break;
          case 10: //群聊专用
            try
            {
              Server.PublicChat(this, pack.ToUnicodeString(1));
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
