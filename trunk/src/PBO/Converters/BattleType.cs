using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Media;
using System.Globalization;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.PBO.Elements;

namespace PokemonBattleOnline.PBO.Converters
{
  public class BattleTypeBg : Converter<BattleType>
  {
    public readonly static BattleTypeBg C = new BattleTypeBg();
    static readonly SolidColorBrush[] c;
    static BattleTypeBg()
    {
      c = new SolidColorBrush[RomData.BattleTypes];
      c[0] = SBrushes.NewBrush(0xffa8a090);
      c[1] = SBrushes.NewBrush(0xffa05038);
      c[2] = SBrushes.NewBrush(0xff98a8f0);
      c[3] = SBrushes.NewBrush(0xffb058a0);
      c[4] = SBrushes.NewBrush(0xffd0b058);
      c[5] = SBrushes.NewBrush(0xffb8a058);
      c[6] = SBrushes.NewBrush(0xffa8b820);
      c[7] = SBrushes.NewBrush(0xff6060b0);
      c[8] = SBrushes.NewBrush(0xffa8a8c0);
      c[9] = SBrushes.NewBrush(0xfff05030);
      c[10] = SBrushes.NewBrush(0xff3898f8);
      c[11] = SBrushes.NewBrush(0xff78c850);
      c[12] = SBrushes.NewBrush(0xfff8c030);
      c[13] = SBrushes.NewBrush(0xfff870a0);
      c[14] = SBrushes.NewBrush(0xff58c8e0);
      c[15] = SBrushes.NewBrush(0xff7860e0);
      c[16] = SBrushes.NewBrush(0xff705848);
      c[17] = SBrushes.NewBrush(0xfff1a7f9);
    }

    protected override object Convert(BattleType value)
    {
      return value == BattleType.Invalid ? null : c[(int)(byte)value - 1];
    }
  }

  public class BattleTypeBorder : Converter<BattleType>
  {
    public static readonly BattleTypeBorder C = new BattleTypeBorder();
    static readonly SolidColorBrush[] c;
    static BattleTypeBorder()
    {
      c = new SolidColorBrush[RomData.BattleTypes];
      c[0] = SBrushes.NewBrush(0xff505050);
      c[1] = SBrushes.NewBrush(0xff483830);
      c[2] = SBrushes.NewBrush(0xff405090);
      c[3] = SBrushes.NewBrush(0xff483850);
      c[4] = SBrushes.NewBrush(0xff705018);
      c[5] = SBrushes.NewBrush(0xff705018);
      c[6] = SBrushes.NewBrush(0xff406838);
      c[7] = SBrushes.NewBrush(0xff483850);
      c[8] = SBrushes.NewBrush(0xff505050);
      c[9] = SBrushes.NewBrush(0xff702008);
      c[10] = SBrushes.NewBrush(0xff405090);
      c[11] = SBrushes.NewBrush(0xff406838);
      c[12] = SBrushes.NewBrush(0xff705018);
      c[13] = SBrushes.NewBrush(0xff683838);
      c[14] = SBrushes.NewBrush(0xff405090);
      c[15] = SBrushes.NewBrush(0xff483890);
      c[16] = SBrushes.NewBrush(0xff483830);
      c[17] = SBrushes.NewBrush(0xffdf39f1);
    }

    protected override object Convert(BattleType value)
    {
      return value == BattleType.Invalid ? null : c[(int)(byte)value - 1];
    }
  }
  public class BattleTypeMoveButton : Converter<BattleType>
  {
    public static readonly BattleTypeMoveButton C = new BattleTypeMoveButton();
    static readonly ImageSource[] c;
    static BattleTypeMoveButton()
    {
      c = new ImageSource[RomData.BattleTypes + 1];
      c[1] = Helper.GetImage(@"ControlPanel/Fight/Normal.png");
      c[2] = Helper.GetImage(@"ControlPanel/Fight/Fighting.png");
      c[3] = Helper.GetImage(@"ControlPanel/Fight/Flying.png");
      c[4] = Helper.GetImage(@"ControlPanel/Fight/Poison.png");
      c[5] = Helper.GetImage(@"ControlPanel/Fight/Ground.png");
      c[6] = Helper.GetImage(@"ControlPanel/Fight/Rock.png");
      c[7] = Helper.GetImage(@"ControlPanel/Fight/Bug.png");
      c[8] = Helper.GetImage(@"ControlPanel/Fight/Ghost.png");
      c[9] = Helper.GetImage(@"ControlPanel/Fight/Steel.png");
      c[10] = Helper.GetImage(@"ControlPanel/Fight/Fire.png");
      c[11] = Helper.GetImage(@"ControlPanel/Fight/Water.png");
      c[12] = Helper.GetImage(@"ControlPanel/Fight/Grass.png");
      c[13] = Helper.GetImage(@"ControlPanel/Fight/Electric.png");
      c[14] = Helper.GetImage(@"ControlPanel/Fight/Psychic.png");
      c[15] = Helper.GetImage(@"ControlPanel/Fight/Ice.png");
      c[16] = Helper.GetImage(@"ControlPanel/Fight/Dragon.png");
      c[17] = Helper.GetImage(@"ControlPanel/Fight/Dark.png");
      c[18] = Helper.GetImage(@"ControlPanel/Fight/Fairy.png");
    }

    protected override object Convert(BattleType value)
    {
      return c[(int)value];
    }
  }
}
