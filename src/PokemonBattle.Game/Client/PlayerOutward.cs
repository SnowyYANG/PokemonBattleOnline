using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game
{
  public class PlayerOutward
  {
    private const byte NO = 0;
    private const byte NORMAL = 1;
    private const byte ABNORMAL = 2;
    private const byte FAINT = 3;

    public readonly int Id;
    
    internal PlayerOutward(int id, string name)
    {
      Id = id;
      Name = name;
    }

    public string Name
    { get; private set; }
    public IEnumerable<byte> Pokemons
    { get { return null; } }
  }
}
