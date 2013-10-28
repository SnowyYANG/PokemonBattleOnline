using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.PBO.Battle
{
  internal abstract class BattleReportImplement
  {
    protected abstract bool Visible(LogText text);
    public abstract void AddText(string text, uint foreground = 0xff000000, Alignment alignment = Alignment.Left, uint background = 0, double size = 0, bool bold = false, bool italic = false, bool underline = false);
    public void AddText(LogText text)
    {
      if (Visible(text))
        if (text.Contents == null) AddText(text.Text, text.Foreground, text.Alignment, text.Background, text.FontSize, text.IsBold, text.IsItalic, text.IsUnderlined);
        else foreach (var t in text.Contents) AddText(t);
    }
  }
}
