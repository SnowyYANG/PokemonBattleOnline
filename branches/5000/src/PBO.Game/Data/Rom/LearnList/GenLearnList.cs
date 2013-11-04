using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PokemonBattleOnline.Game
{
  public class GameLearnList
  {
    public readonly string Name;
    public readonly LvLearnList Lv;
    public readonly EggLearnList Egg;
    public readonly TMHMTutorLearnList TM;
    public readonly TMHMTutorLearnList Tutor;
    public readonly TMHMTutorLearnList HM;

    public GameLearnList(string name, LvLearnList lv, EggLearnList egg, TMHMTutorLearnList tm, TMHMTutorLearnList tutor, TMHMTutorLearnList hm)
    {
      Name = name;
      Lv = lv;
      Egg = egg;
      TM = tm;
      Tutor = tutor;
      HM = hm;
    }
  }
  public class GenLearnList
  {
    public readonly int Gen;

    internal GenLearnList(int gen)
    {
      Gen = gen;
      games = new Dictionary<string, GameLearnList>();
    }

    public IEnumerable<LvLearnList> Lvs
    { get; private set; }
    public IEnumerable<EggLearnList> Eggs
    { get; private set; }
    public IEnumerable<TMHMTutorLearnList> TMHMTutors
    { get; private set; }
    private readonly Dictionary<string, GameLearnList> games;
    public IEnumerable<GameLearnList> Games
    { get { return games.Values; } }

    internal void SetAll(LvLearnList[] lvs, EggLearnList[] eggs, TMHMTutorLearnList[] tmhmts)
    {
      Lvs = lvs;
      Eggs = eggs;
      TMHMTutors = tmhmts;
    }

    internal void Add(string game, LvLearnList lv, EggLearnList egg, TMHMTutorLearnList tm, TMHMTutorLearnList tutor, TMHMTutorLearnList hm)
    {
      games.Add(game, new GameLearnList(game, lv, egg, tm, tutor, hm));
    }
  }
}
