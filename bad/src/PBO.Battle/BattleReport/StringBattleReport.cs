using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.PBO.Battle
{
  internal class StringBattleReport : BattleReportImplement
  {
    private readonly StringBuilder StringBuilder;
    
    public StringBattleReport(StringBuilder str)
    {
      StringBuilder = str;
    }
    
    public override void AddText(string text, uint foreground = 0xff000000, Tactic.DataModels.Alignment alignment = Tactic.DataModels.Alignment.Left, uint background = 0, double size = 0, bool bold = false, bool italic = false, bool underline = false)
    {
      StringBuilder.Append(text);
    }
    protected override bool Visible(Game.IText text)
    {
      return !text.HiddenAfterBattle;
    }
  }
}
