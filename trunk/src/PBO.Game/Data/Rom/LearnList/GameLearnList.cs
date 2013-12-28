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
}
