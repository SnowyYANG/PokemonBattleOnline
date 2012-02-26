using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;
using LightStudio.PokemonBattle.PBO.Battle.Board3D;

namespace LightStudio.PokemonBattle.PBO.Battle
{
  /// <summary>
  /// Interaction logic for BattleField3D.xaml
  /// </summary>
  public partial class BattleField3D : UserControl
  {
    BoardOutward board;
    Terrain3D owner, rival;
    TerrainBG terrainBg;
    Vector3D lookDirection;
    Point3D cameraPosition;

    public BattleField3D()
    {
      InitializeComponent();
      lookDirection = camera.LookDirection;
      cameraPosition = camera.Position;
      this.Loaded += (sender, e) => { this.Focus(); };
    }

    public void Init(BoardOutward board)//internal...
    {
      this.board = board;
      owner = new Terrain3D(5, 3, board.Terrain);
      rival = new Terrain3D(-5, 3, board.Terrain);
      m3dgroup.Children.Add(owner.Model);
      m3dgroup.Children.Add(rival.Model);
      //m3dgroup.Children.Add(new Pokemon().Model);
    }

    private void UserControl_KeyDown(object sender, KeyEventArgs e)
    {
      switch (e.Key)
      {
        case Key.PageUp:
          camera.FieldOfView -= 1;
          break;
        case Key.PageDown:
          camera.FieldOfView += 1;
          break;
        case Key.J:
          lookDirection.Z /= 1.2;
          break;
        case Key.L:
          lookDirection.Z *= 1.2;
          break;
        case Key.I:
          lookDirection.Y += 0.1;
          break;
        case Key.K:
          lookDirection.Y -= 0.1;
          break;
        case Key.A:
          cameraPosition.X -= 1;
          break;
        case Key.D:
          cameraPosition.X += 1;
          break;
        case Key.W:
          cameraPosition.Z += 1;
          break;
        case Key.S:
          cameraPosition.Z -= 1;
          break;
        case Key.Up:
          cameraPosition.Y += 1;
          break;
        case Key.Down:
          cameraPosition.Y -= 1;
          break;
      }
      camera.Position = cameraPosition;
      camera.LookDirection = lookDirection;
    }
  }
}
