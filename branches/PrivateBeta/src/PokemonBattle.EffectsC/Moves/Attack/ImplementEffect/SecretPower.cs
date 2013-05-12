using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Attack
{
  class SecretPower : AttackMoveE
  {
    public SecretPower(int id)
      : base(id)
    {
    }
    protected override void ImplementEffect(DefContext def)
    {
      if (def.RandomHappen(30))
        switch (def.Defender.Controller.GameSettings.Terrain)
        {
          case Terrain.Path:
            def.Defender.AddState(def.AtkContext.Attacker, Data.AttachedState.PAR, false);
            break;
        }
    }
  }
}
