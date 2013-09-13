using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Game.GameEvents
{
  [DataContract(Namespace = PBOMarks.PBO)]
  public class Mimic : GameEvent
  {
    [DataMember]
    public int Pm;
    [DataMember]
    public int Move;

    protected override void Update()
    {
      AppendGameLog("Mimic", Pm, Move);
    }
    public override void Update(SimGame game)
    {
      var pm = GetOnboardPokemon(game, Pm);
      if (pm != null && pm.Pokemon.Owner == game.Player) pm.ChangeMove(Ms.MIMIC, Move);
    }
  }
}
