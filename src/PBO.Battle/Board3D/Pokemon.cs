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
  class Pokemon3D
  {
    static readonly PointCollection TEXTURE_COORDINATES;
    static readonly Int32Collection TRIANGLE_INDICES;
    static Pokemon3D()
    {
      TEXTURE_COORDINATES = new PointCollection(new Point[]
        { new Point(0, 1), new Point(0, 0), new Point(1, 1), new Point(1, 0) });
      TRIANGLE_INDICES = new Int32Collection(new int[] { 0, 2, 1, 3, 1, 2 });
      TEXTURE_COORDINATES.Freeze();
      TRIANGLE_INDICES.Freeze();
    }

    public readonly Model3D Model;
    PokemonOutward pokemon;

    public Pokemon3D(PokemonOutward pokemon)
    {
      MeshGeometry3D mesh = new MeshGeometry3D();
      mesh.Positions = new Point3DCollectionConverter().ConvertFromString("0,0,-1 0,1,-1 1,0,-1 1,1,-1") as Point3DCollection;
      mesh.TriangleIndices = TRIANGLE_INDICES;
      mesh.TextureCoordinates = TEXTURE_COORDINATES;
      Model = new GeometryModel3D(mesh, new DiffuseMaterial(Brushes.Red));
    }

    void pokemon_Hurt(object sender, EventArgs e)
    {
      throw new NotImplementedException();
    }
  }
}
