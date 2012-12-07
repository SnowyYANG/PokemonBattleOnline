using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game
{
  public class PlayerOutward
  {
    public readonly int Id;
    
    internal PlayerOutward(int id, string name)
    {
      Id = id;
      Name = name;
    }

    public string Name
    { get; private set; }
  }
}
