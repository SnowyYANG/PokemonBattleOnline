using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PokemonBattleOnline.Game
{
  public class GameLearnset
  {
    public readonly string Name;
    public readonly LvLearnset Lv;
    public readonly EggLearnset Egg;
    public readonly TMHMTutorLearnset TM;
    public readonly TMHMTutorLearnset Tutor;
    public readonly TMHMTutorLearnset HM;

    public GameLearnset(string name, LvLearnset lv, EggLearnset egg, TMHMTutorLearnset tm, TMHMTutorLearnset tutor, TMHMTutorLearnset hm)
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
