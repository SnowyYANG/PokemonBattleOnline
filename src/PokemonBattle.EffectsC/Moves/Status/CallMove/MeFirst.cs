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

    protected override bool NotFail(AtkContext atk)
    {
      if (base.NotFail(atk))
      {
        var der = atk.Target.Defender;
        var dm = der.SelectedMove;
        return
          !(
          der.LastMoveTurn == der.Controller.TurnNumber || der.OnboardPokemon.HasCondition("SkyDrop") ||
          dm == null ||
          dm.Type.Category == Data.MoveCategory.Status || dm.Type.Id == Sp.Moves.STRUGGLE || Sp.Moves.FocusPunch(dm)
          );
      }
      return false;
    }
    public override void Execute(AtkContext atk)
    {
      atk.SetTurnCondition("MeFirst");
      if (NotFail(atk)) CallMove(atk, atk.Target.Defender.SelectedMove.Type, atk.Target.Defender.Tile);
      else FailAll(atk);
    }
  }
}
