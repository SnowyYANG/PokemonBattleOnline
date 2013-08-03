using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using PokemonBattleOnline.Data;
using PokemonBattleOnline.Game.Host;

namespace PokemonBattleOnline.Game.GameEvents
{
  [DataContract(Name = "em", Namespace = PBOMarks.PBO)]
  public class UseMove : GameEvent
  {
    [DataMember(Name = "a")]
    int Pm;
    [DataMember(Name = "b")]
    int Move;
    [DataMember(Name = "c", EmitDefaultValue = false)]
    public int PP;

    internal UseMove(PokemonProxy pm, MoveType move)
    {
      Pm = pm.Id;
      Move = move.Id;
    }

    protected override void Update()
    {
      var pm = Game.GetPokemon(Pm);
      pm.ChangePosition(pm.Position.X, CoordY.Plate);
      if (Move == Host.Ms.STRUGGLE) AppendGameLog("Struggle", Pm);
      AppendGameLog("UseMove", Pm, Move);
    }
    public override void Update(SimGame game)
    {
      var pm = GetOnboardPokemon(game, Pm);
      if (pm != null)
        foreach (var m in pm.Moves)
          if (m != null && m.Type.Id == Move)
          {
            m.PP.Value -= 1 + PP;
            break;
          }
    }
  }
}
