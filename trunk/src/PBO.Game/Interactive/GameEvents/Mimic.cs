using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using PokemonBattleOnline.Game.Host;

namespace PokemonBattleOnline.Game.GameEvents
{
  [DataContract(Namespace = PBOMarks.PBO)]
  class Mimic : GameEvent
  {
    [DataMember]
    int Pm;
    [DataMember]
    int Move;

    public Mimic(PokemonProxy pm, Data.MoveType move)
    {
      Pm = pm.Id;
      Move = move.Id;
    }
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
