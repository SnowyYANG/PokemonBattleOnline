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

    protected internal override void FilterDefContext(AtkContext atk)
    {
      base.FilterDefContext(atk);
      if (atk.Targets != null)
      {
        var targets = atk.Targets.ToList();
        if (!Move.AdvancedFlags.IgnoreSubstitute)
          foreach (DefContext d in targets.ToArray())
            if (d.Defender != atk.Attacker && d.Defender.OnboardPokemon.HasCondition("Substitute"))
            {
              Fail(d);
              targets.Remove(d);
            }
        if (Move.Class == MoveInnerClass.ForceToSwitch)
          foreach(DefContext d in targets.ToArray())
            if (d.Defender.OnboardPokemon.HasCondition("Ingrain"))
            {
              d.Defender.AddReportPm("IngrainCantMove");
              targets.Remove(d);
            }
        atk.SetTargets(targets);
      }
    }
    protected override void Act(AtkContext atk) //1、2、3、5、A、B、C、D
    {
      bool notAllFail = false;
      switch (Move.Class)
      {
        case MoveInnerClass.AddState:
          foreach (var d in atk.Targets) notAllFail |= d.Defender.AddState(d);
          if (atk.Move.Attachment.State == AttachedState.PerishSong)
            if (notAllFail) atk.Controller.ReportBuilder.Add("EnPerishSong");
            else FailAll(atk);
          break;
        case MoveInnerClass.Lv7DChange:
          foreach (var d in atk.Targets) notAllFail |= d.Defender.ChangeLv7D(atk);
          atk.FailAll = !notAllFail;
          break;
        case MoveInnerClass.HpRecover:
          foreach (var d in atk.Targets)
            d.Defender.HpRecover(d.Defender.Pokemon.Hp.Origin * atk.Move.MaxHpPercentage / 100, true);
          break;
        case MoveInnerClass.ConfusionWithLv7DChange:
          atk.Target.Defender.AddState(atk.Target);
          atk.Target.Defender.ChangeLv7D(atk);
          break;
        case MoveInnerClass.ForceToSwitch:
          int aLv = atk.Attacker.Pokemon.Lv, dLv = atk.Target.Defender.Pokemon.Lv;
          if ((aLv < dLv && (aLv + dLv) * atk.Controller.GetRandomInt(0, 255) < dLv >> 2) || !ForceSwitch(atk.Target))
            FailAll(atk);
          break;
      }
    }
  }
}