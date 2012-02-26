using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LightStudio.PokemonBattle.Game
{
  public enum CoordY : byte
  {
    Plate,
    Air,
    Water,
    Underground,
    Another
  }
  /// <summary>
  /// reference type, dont share among pokemons
  /// </summary>
  [DataContract(Namespace=Namespaces.DEFAULT)]
  public class Position
  {
    [DataMember(EmitDefaultValue = false)]
    public readonly int Team;
    [DataMember(EmitDefaultValue = false)]
    public int X; //转盘用负数横坐标会不会很有趣
    [DataMember(EmitDefaultValue = false)]
    public CoordY Y;

    public Position(int team, int x, CoordY y = CoordY.Plate)
    {
      Team = team;
      X = x;
      Y = y;
    }
  }
}
