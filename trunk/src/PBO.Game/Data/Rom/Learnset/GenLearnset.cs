using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PokemonBattleOnline.Game
{
  public class GenLearnset
  {
    public readonly int Gen;

    internal GenLearnset(int gen)
    {
      Gen = gen;
      games = new Dictionary<string, GameLearnset>();
    }

    private readonly Dictionary<string, GameLearnset> games;
    public IEnumerable<GameLearnset> Games
    { get { return games.Values; } }

    internal void Add(string game, LvLearnset lv, EggLearnset egg, TMHMTutorLearnset tm, TMHMTutorLearnset tutor, TMHMTutorLearnset hm)
    {
      games.Add(game, new GameLearnset(game, lv, egg, tm, tutor, hm));
    }
  }
}
