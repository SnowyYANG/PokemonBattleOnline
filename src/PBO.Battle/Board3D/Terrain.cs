using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using LightStudio.PokemonBattle.Game;
using Point = System.Windows.Point;

namespace LightStudio.PokemonBattle.PBO.Battle.Board3D
{
  /// <summary>
  /// 以x轴为对称轴在y=0平面的正十六边形
  /// </summary>
  class Terrain3D
  {
    static readonly double[] SX;
    static readonly double[] SZ;
    static readonly PointCollection TEXTURE_COORDINATES;
    static readonly Int32Collection TRIANGLE_INDICES;
    static Terrain3D()
    {
      {
        SX = new double[17];
        SZ = new double[17];
        int i = 0;
        for (double d = 0; d < 2; d += 0.125)
        {
          i++;
          SX[i] = Math.Cos(Math.PI * d);
          SZ[i] = Math.Sin(Math.PI * d);
        }
      }
      {
        TEXTURE_COORDINATES = new PointCollection(17);
        TEXTURE_COORDINATES.Add(new Point(0, 0));
        Point p = new Point(1, 0);
        int i = -1;
        while (++i < 16) TEXTURE_COORDINATES.Add(p);
        TEXTURE_COORDINATES.Freeze();
      }
      {
        TRIANGLE_INDICES = new Int32Collection(48);
        for (int i = 1; i < 16; i++)
        {
          TRIANGLE_INDICES.Add(0);
          TRIANGLE_INDICES.Add(i + 1);
          TRIANGLE_INDICES.Add(i);
        }
        TRIANGLE_INDICES.Add(0);
        TRIANGLE_INDICES.Add(1);
        TRIANGLE_INDICES.Add(16);
        TRIANGLE_INDICES.Freeze();
      }
    }
    static Material GetMaterial(Terrain terrain)
    {
      return new DiffuseMaterial(Brushes.Blue);
    }
    
    public readonly Model3D Model;
    
    /// <summary>
    /// 为了旋转战斗，两个场地还是要用两个模型
    /// </summary>
    /// <param name="x"></param>
    /// <param name="r"></param>
    /// <param name="terrain"></param>
    public Terrain3D(double z, double r, Terrain terrain)
    {
      MeshGeometry3D mesh = new MeshGeometry3D();
      Point3D p3d = new Point3D();
      p3d.Y = 0;
      for (int i = 0; i < 17; i++)
      {
        p3d.X = SX[i] * r;
        p3d.Z = SZ[i] * r + z;
        mesh.Positions.Add(p3d);
      }
      mesh.TextureCoordinates = TEXTURE_COORDINATES;
      mesh.TriangleIndices = TRIANGLE_INDICES;
      Model = new GeometryModel3D(mesh, GetMaterial(terrain));
    }

    public void Clockwise()
    {
    }
    public void Counterclockwise()
    {
    }
  }
}
