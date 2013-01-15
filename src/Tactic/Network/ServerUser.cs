using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using PokemonBattleOnline.Tactic.Network.Commands;

namespace PokemonBattleOnline.Tactic.Network
{
  public class ServerUser<TE, TUE> : UserBase
  {
    private readonly Server<TE, TUE> Server;

    internal ServerUser(LoginUser<TUE> user, Server<TE, TUE> server)
      : base(user.Network)
    {
      //Network.Removed += () => Server.RemoveUser(this);
      _user = new User<TUE>(user.Network.Id, user.Name, user.Extension);
      Server = server;
    }

    private readonly User<TUE> _user;
    public User<TUE> User
    { get { return _user; } }

    protected override void OnPackReceived(byte[] pack)
    {
      if (!pack.IsEmpty())
      {
        var h = pack.GetHeader();
        var c = pack.GetContent();
        switch (h)
        {
          case 0:
            Serializer.DeserializeFromJson<UserCommand<TE, TUE>>(c).Execute(this);
            break;
          case 1: //p2p
            {
              var n = c[0];
              var receivers = new int[n]; //n = 0/all
              var offset = 1;
              for (int i = 0; i < n; ++i)
              {
                receivers[i] = c.SubArray(offset, 2).ToInt16().Value;
                offset += 2;
              }
              Server.SendP2PPack(this, c.SubArray(offset), receivers);
            }
            break;
          case 10: //群聊作弊专用
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
