using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Status
{
  class MeFirst : AttackMoveE
  {
    public MeFirst(int id)
      : base(id)
    {
    }
    
    protected override bool NotFailOnTarget(DefContext def)
    {
      var dm = def.Defender.SelectedMove;
      return
        def.Defender.LastMoveTurn != def.Defender.Controller.TurnNumber && dm != null &&
        dm.Move.Type.Category != Data.MoveCategory.Status && dm.Move.Type.Id != Sp.Moves.STRUGGLE && Sp.Moves.FocusPunch(dm);
    }
  }
}
