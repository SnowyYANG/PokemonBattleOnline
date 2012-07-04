using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  public class StatusMoveE : MoveE
  {
    public StatusMoveE(int moveId)
      : base(moveId)
    {
    }

    protected virtual void Act(AtkContext atk) //1、2、3、5、A、B、C、D
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
      //来个摇手指啥的，基本可以xsk了？摇手指随机一个id，然后再调用Execute吧
      if (pm.AtkContext == null) pm.BuildAtkContext(Move);
      pm.OnboardPokemon.CoordY = CoordY.Plate;
      //battletype
      //targets
      pm.Action = PokemonAction.Done;
    }
  }
}