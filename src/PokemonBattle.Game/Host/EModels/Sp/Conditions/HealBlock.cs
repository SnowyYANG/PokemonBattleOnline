using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Sp.Conditions
{
  public class HealBlock : PmCondition
  {
    public HealBlock(PokemonProxy pm)
      : base("HealBlock", pm, 5)
    {
    }

    public override bool CanExecute()
    {
      if (pm.SelectedMove.Move.Type.AdvancedFlags.IsHeal)
      {
        AddReportPm("HealBlock");
        return false;
      }
      return true;
    }

    public override void EndTurn()
    {
      if (--count <= 0) Remove();
    }
  }
}
