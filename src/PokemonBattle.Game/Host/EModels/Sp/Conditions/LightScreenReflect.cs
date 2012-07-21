using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Host.Sp.Conditions
{
  public static class LightScreenReflect
  {
    public static Modifier DamageFinalModifier(DefContext def)
    {
      //If the target's side is affected by Reflect, the move used was physical, the user's ability isn't Infiltrator and the critical hit flag isn't set. 
      //The value of the modificator is 0xA8F if there is more than one Pokemon per side of the field and 0x800 otherwise.
      //Same as above with Light Screen and special moves.
      ushort m = 0x1000;
      if (
        (def.AtkContext.Move.Category == MoveCategory.Physical && def.HasInfiltratableCondition("Reflect")) ||
        (def.AtkContext.Move.Category == MoveCategory.Special && def.HasInfiltratableCondition("LightScreen")))
      {
        if (def.Defender.Controller.Game.Settings.Mode.XBound() > 1) m = 0xA8F; //seriously? am I sure?!
        else m = 0x800;
      }
      return m;
    }
  }
}
