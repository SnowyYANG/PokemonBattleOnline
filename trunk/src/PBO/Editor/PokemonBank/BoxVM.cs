using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.PBO.Editor
{
  internal class BoxVM : BTVM
  {
    public static readonly object ICON;
    private static readonly object ICONOPEN;
    static BoxVM()
    {
      ICON = Helper.GetImage(@"Balls/Dive.png");
      ICONOPEN = Helper.GetImage(@"Balls/DiveOpen.png");
    }

    public BoxVM(PokemonBT model)
      : base(model)
    {
    }

    public override object Icon
    { get { return IsOpen ? ICONOPEN : ICON; } }
    public override object BorderBrush
    { get { return PBO.Elements.SBrushes.BlueM; } }
    public override object Effect
    { get { return Model.Any() ? null : R.OrangeShadow; } }

    protected override void Remove()
    {
      base.Remove();
      UserData.Current.Boxes.Remove((PokemonBT)Model);
    }
  }
}
