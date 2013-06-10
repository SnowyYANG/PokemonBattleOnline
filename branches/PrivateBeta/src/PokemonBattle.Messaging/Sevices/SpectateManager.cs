using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.Tactic.Messaging;

namespace LightStudio.PokemonBattle.Messaging
{
  public class SpectateManager : ClientService
  {
    private readonly BattleClient Battle;

    internal SpectateManager(BattleClient battle)
      : base(battle.Client)
    {
      Battle = battle;
    }

    protected override void ReadMessage(User user, byte header, System.IO.BinaryReader reader)
    {
    }
    public void Spectate(User user)
    {
      if (Battle.CanEnterRoom)
      {
      }
    }
  }
}
