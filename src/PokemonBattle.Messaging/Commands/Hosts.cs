using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Tactic.Network;
using PokemonBattleOnline.Messaging.Room;
using User = PokemonBattleOnline.Tactic.Network.User<PokemonBattleOnline.Messaging.UE>;

namespace PokemonBattleOnline.Messaging
{
  //public class Hosts : ClientService
  //{
  //  private Host host;
    
  //  internal Hosts(Client client)
  //    : base(client, MessageHeaders.GAME_C2H)
  //  {
  //  }

  //  protected override void ReadMessage(User user, byte header, System.IO.BinaryReader reader)
  //  {
  //    lock(this)
  //      if (host != null)
  //      {
  //        string h = reader.ReadString();
  //        var message = new TextMessage(h, reader.ReadString());
  //        HostCommand cmd = message.GetMessageObjectOrNull() as HostCommand;
  //        if (cmd != null) ((IHost)host).ExecuteCommand(cmd, user.Id);
  //      }
  //  }

  //  public bool AddHost(GameInitSettings settings, bool auto)
  //  {
  //    lock(this)
  //      if (host == null)
  //      {
  //        host = new Host(settings, auto);
  //        host.SendInformation += host_SendInformation;
  //        host.Closed += () =>
  //          {
  //            host.Dispose();
  //            host = null;
  //            Client.DeregisterRoom();
  //          };
  //        Client.RegisterRoom();
  //        return true;
  //      }
  //    return false;
  //  }
  //  private void host_SendInformation(UserInformation info, int[] receivers)
  //  {
  //    SendMessage(MessageHeaders.GAME_H2C, info, receivers);
  //  }
  //}
}
