using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PokemonBattleOnline.Game
{
  public class GenLearnList
  {
    public readonly int Gen;

    internal GenLearnList(int gen)
    {
      Gen = gen;
      games = new Dictionary<string, GameLearnList>();
    }

    private readonly Dictionary<string, GameLearnList> games;
    public IEnumerable<GameLearnList> Games
    { get { return games.Values; } }

    internal void Add(string game, LvLearnList lv, EggLearnList egg, TMHMTutorLearnList tm, TMHMTutorLearnList tutor, TMHMTutorLearnList hm)
    {
      games.Add(game, new GameLearnList(game, lv, egg, tm, tutor, hm));
    }
  }
}
