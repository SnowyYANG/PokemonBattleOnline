using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  public class StatusMoveE : MoveE
  {
    public StatusMoveE(MoveType move)
      : base(move)
    {
    }

    protected virtual void Act() //1、2、3、5、A、B、C、D
    {
      switch (Move.Class)
      {
        case MoveInnerClass.ConfusionWithLv7DChange:
          break;
        case MoveInnerClass.ForceToShift:
          break;
        case MoveInnerClass.HpRecover:
          break;
        case MoveInnerClass.Lv7DChange:
          break;
      }
    }

    public override void Execute(PokemonProxy pm)
    {
      System.Diagnostics.Debugger.Break();
      pm.Action = PokemonAction.Done;
    }
  }
}