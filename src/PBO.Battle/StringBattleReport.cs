using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.PBO.Battle
{
  internal class StringBattleReport : BattleReportImplement
  {
    public override void AddText(string text, uint foreground = 0xff000000, Tactic.DataModels.Alignment alignment = Tactic.DataModels.Alignment.Left, uint background = 0, double size = 0, bool bold = false, bool italic = false, bool underline = false)
    {
      throw new NotImplementedException();
    }
    protected override bool Visible(Game.IText text)
    {
      throw new NotImplementedException();
    }
  }
}
