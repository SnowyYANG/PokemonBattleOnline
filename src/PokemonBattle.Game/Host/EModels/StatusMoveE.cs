using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Interactive;
using LightStudio.PokemonBattle.Interactive.GameEvents;
using LightStudio.PokemonBattle.Game.Sp;

namespace LightStudio.PokemonBattle.Game
{
  public class StatusMoveE : MoveE
  {
    public StatusMoveE(int moveId)
      : base(moveId)
    {
    }

    protected override void Act(AtkContext atk) //1、2、3、5、A、B、C、D
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
      AtkContext atk = pm.AtkContext;
      if (!Abilities.CalculateType(atk)) CalculateType(atk);
      //targets
      pm.Action = PokemonAction.Done;
    }
  }
}