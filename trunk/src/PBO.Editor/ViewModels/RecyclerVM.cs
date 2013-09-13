using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Tactic.Globalization;
using PokemonBattleOnline.PBO.UIElements;

namespace PokemonBattleOnline.PBO.Editor
{
  internal class RecyclerVM : CollectionVM
  {
    static readonly object OPENICON;
    static readonly object ICON;
    static RecyclerVM()
    {
      OPENICON = PBO.Helper.GetImage(@"Balls/NetOpen.png");
      ICON = PBO.Helper.GetImage(@"Balls/Net.png");
    }

    public RecyclerVM(PokemonRecycler model)
      : base(model)
    {
    }

    public override object Icon
    {
      get
      {
        if (IsOpen) return OPENICON;
        return ICON;
      }
    }
    public override object BorderBrush
    { get { return System.Windows.Media.Brushes.Gray; } }
    public override object Background
    { get { return @"#25110E"; } }
    public override object Effect
    { get { return null; } }
  }
}
