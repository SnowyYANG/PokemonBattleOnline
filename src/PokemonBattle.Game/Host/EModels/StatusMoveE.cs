using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.GameEvents;
using LightStudio.PokemonBattle.Game.Host.Sp;

namespace LightStudio.PokemonBattle.Game.Host
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
        case MoveInnerClass.AddState:
          foreach (var d in atk.Targets) d.Defender.AddState(atk);
          break;
        case MoveInnerClass.Lv7DChange:
          foreach (var d in atk.Targets) d.Defender.ChangeLv7D(atk);
          break;
        case MoveInnerClass.HpRecover:
          foreach (var d in atk.Targets)
          {
            if (d.Defender.Hp == d.Defender.Pokemon.Hp.Origin) d.Defender.AddReportPm("FullHp"); //不在场和濒死都是不可能的
            else d.Defender.HpRecover(d.Defender.Pokemon.Hp.Origin * atk.Move.MaxHpPercentage / 100);
          }
          break;
        case MoveInnerClass.ConfusionWithLv7DChange:
          break;
        case MoveInnerClass.ForceToShift:
          break;
      }
    }
  }
}