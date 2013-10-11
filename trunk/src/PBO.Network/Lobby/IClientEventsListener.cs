using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Network
{
  public interface IClientEventsListener
  {
    void Disconnected();
    void UpdateUser(User user);
    void RemoveUser(int id);
    void PublicChat(string chat, User from);
    void PrivateChat(string chat, User from);
    void RoomChat(string chat, User from);
  }
}
