using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.Tactic.Messaging;
using IUser = LightStudio.Tactic.Messaging.IUser<LightStudio.PokemonBattle.Messaging.RoomInfo>;

namespace LightStudio.PokemonBattle.Messaging
{
  public class SpectateManager : ClientService
  {
    internal SpectateManager(BattleClient battle)
      : base(battle.Client, MessageHeaders.SPECTATE)
    {
      
    }
    protected override void ReadMessage(IUser user, byte header, System.IO.BinaryReader reader)
    {
      throw new NotImplementedException();
    }
    public void Spectate()
    {
    }
  }
}
