using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace PokemonBattleOnline.PBO.UIElements
{
  /// <summary>
  /// [ui] B:Background F:Foreground BHL:BackgroundHightlight FHL:ForegroundHighlight
  /// [game] M:Main
  /// </summary>
  public static class Brushes
  {
    public static readonly Brush BlueF;
    public static readonly Brush BlueFHL;
    public static readonly Brush CyanF;
    public static readonly Brush GrayB1;
    public static readonly Brush GrayF;

    public static readonly Brush SelectedItemBg;

    public static readonly Brush BlueM;
    public static readonly Brush OrangeM;
    public static readonly Brush MagentaM;

    static Brushes()
    {
      BlueF = Helper.NewBrush(0xff085888);
      BlueFHL = Helper.NewBrush(0xff58c0e8);
      CyanF = Helper.NewBrush(0xff28c0a8);
      GrayB1 = Helper.NewBrush(0xff181818);
      GrayF = Helper.NewBrush(0xfff8f8f8);

      SelectedItemBg = Helper.NewBrush(0x804294de);

      BlueM = Helper.NewBrush(0xff0080e8);
      OrangeM = Helper.NewBrush(0xffe88800);
      MagentaM = Helper.NewBrush(0xffd82038);
    }

    public static Brush GetGridTileBrush(double size, Brush Color)
    {
      DrawingBrush b = new DrawingBrush();
      GeometryGroup gg = new GeometryGroup();
      gg.Children.Add(new LineGeometry(new Point(0.5, 0), new Point(0.5, size)));
      gg.Children.Add(new LineGeometry(new Point(1, 0.5), new Point(size, 0.5)));
      b.Stretch = Stretch.None;
      b.Drawing = new GeometryDrawing(null, new Pen(Color, 1), gg);
      b.Viewport = new Rect(0, 0, size, size);
      b.ViewportUnits = BrushMappingMode.Absolute;
      b.TileMode = TileMode.Tile;
      return b;
    }
    public static Brush GetHorizontalTileBrush(double size, Brush Color)
    {
      DrawingBrush b = new DrawingBrush();
      DrawingGroup dg = new DrawingGroup();
      dg.Children.Add(new GeometryDrawing(null, new Pen(Color, 1), new LineGeometry(new Point(0, 0.5), new Point(1, 0.5))));
      b.Stretch = Stretch.None;
      b.Drawing = dg;
      b.Viewport = new Rect(0, 0, 1, size);
      b.ViewportUnits = BrushMappingMode.Absolute;
      b.TileMode = TileMode.Tile;
      return b;
    }
  }
}
