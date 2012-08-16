using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Sp.Conditions
{
  public class HealBlock : PmCondition
  {
    public HealBlock(PokemonProxy pm)
      : base("HealBlock", pm, 5)
    {
    }

    public override bool CanExecute()
    {
      if (Pm.SelectedMove.Move.Type.AdvancedFlags.IsHeal)
      {
        AddResetYReport("HealBlock");
        return false;
      }
      return true;
    }
  }
}
