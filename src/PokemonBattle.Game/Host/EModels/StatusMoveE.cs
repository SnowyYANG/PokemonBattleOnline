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

    protected override void CalculateTargets(AtkContext atk)
    {
      base.CalculateTargets(atk);
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
        atk.SetTargets(targets);
      }
    }
    protected override void Act(AtkContext atk) //1、2、3、5、A、B、C、D
    {
      switch (Move.Class)
      {
        case MoveInnerClass.AddState:
          bool notAllFail = false;
          foreach (var d in atk.Targets) notAllFail |= d.Defender.AddState(d);
          if (atk.Move.Attachment.State == AttachedState.PerishSong)
            if (notAllFail) atk.Controller.ReportBuilder.Add("EnPerishSong");
            else FailAll(atk);
          break;
        case MoveInnerClass.Lv7DChange:
          foreach (var d in atk.Targets) d.Defender.ChangeLv7D(atk);
          break;
        case MoveInnerClass.HpRecover:
          foreach (var d in atk.Targets)
            if (atk.Move.AdvancedFlags.IsHeal && d.Defender.OnboardPokemon.HasCondition("HealBlock")) d.Defender.AddReportPm("HealBlock");
            else if (d.Defender.Hp == d.Defender.Pokemon.Hp.Origin) d.Defender.AddReportPm("FullHp"); //不在场和濒死都是不可能的
            else d.Defender.HpRecover(d.Defender.Pokemon.Hp.Origin * atk.Move.MaxHpPercentage / 100);
          break;
        case MoveInnerClass.ConfusionWithLv7DChange:
          break;
        case MoveInnerClass.ForceToShift:
          int aLv = atk.Attacker.Pokemon.Lv, dLv = atk.Target.Defender.Pokemon.Lv;
          if ((aLv < dLv && (aLv + dLv) * atk.Controller.GetRandomInt(0, 255) < dLv >> 2) || !ForceSwitch(atk.Target))
            FailAll(atk);
          break;
      }
    }
  }
}