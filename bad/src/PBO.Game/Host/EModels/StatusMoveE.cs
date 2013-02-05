using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Data;
using PokemonBattleOnline.Game.GameEvents;
using PokemonBattleOnline.Game.Host.Sp;

namespace PokemonBattleOnline.Game.Host
{
  public class StatusMoveE : MoveE
  {
    private static void MagicCoat(AtkContext atk)
    {
      var list = atk.GetCondition<List<PokemonProxy>>("MagicCoat");
      if (list != null)
      {
        atk.RemoveCondition("MagicCoat");
        foreach (var d in list)
        {
          var a = new AtkContext(d);
          a.SetCondition("IgnoreMagicCoat");
          a.StartExecute(atk.Move, atk.Attacker.Tile, d.RaiseAbility(Abilities.MAGIC_BOUNCE) ? "MagicBounce" : "MagicCoat");
          if (atk.Target == null) break;
        }
      }
    }
    
    public StatusMoveE(int moveId)
      : base(moveId)
    {
    }

    public override void Execute(AtkContext atk)
    {
      if (Move.Flags.Snatchable)
      {
        foreach(var pm in atk.Controller.ActingPokemons)
          if (pm.OnboardPokemon.HasCondition("Snatch"))
          {
            pm.OnboardPokemon.RemoveCondition("Snatch");
            pm.AddReportPm("Snatch", atk.Attacker);
            var s = new AtkContext(pm) { Move = Move };
            InitAtkContext(s);
            s.BuildDefContext(null);
            if (NotFail(s)) Act(s);
            else s.FailAll();
            atk.SetAttackerAction(PokemonAction.Done);
            return;
          }
      }
      if (atk.Targets == null && atk.Move.Flags.MagicCoat && atk.Controller.GetOnboardPokemons(1 - atk.Attacker.Pokemon.TeamId).Any((p) => Triggers.MagicCoat(atk, p)))
      {
        atk.FailAll(null);
        MagicCoat(atk);
      }
      else
      {
        base.Execute(atk);
        if (atk.Target == null) MagicCoat(atk);
      }
    }
    protected internal override void FilterDefContext(AtkContext atk)
    {
      base.FilterDefContext(atk);
      if (atk.Target != null)
      {
        var targets = atk.Targets.ToList();
        if (!Move.Flags.IgnoreSubstitute)
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
            else atk.FailAll();
          break;
        case MoveInnerClass.Lv7DChange:
          foreach (var d in atk.Targets) notAllFail |= d.Defender.ChangeLv7D(d);
          atk.Fail = !notAllFail;
          break;
        case MoveInnerClass.HpRecover:
          foreach (var d in atk.Targets)
            d.Defender.HpRecover(d.Defender.Pokemon.Hp.Origin * atk.Move.MaxHpPercentage / 100, true);
          break;
        case MoveInnerClass.ConfusionWithLv7DChange:
          atk.Target.Defender.AddState(atk.Target);
          atk.Target.Defender.ChangeLv7D(atk.Target);
          break;
        case MoveInnerClass.ForceToSwitch:
          int aLv = atk.Attacker.Pokemon.Lv, dLv = atk.Target.Defender.Pokemon.Lv;
          if ((aLv < dLv && (aLv + dLv) * atk.Controller.GetRandomInt(0, 255) < dLv >> 2) || !ForceSwitch(atk.Target)) atk.FailAll();
          break;
      }
    }
    protected override void MoveEnding(AtkContext atk)
    {
      MagicCoat(atk);
      base.MoveEnding(atk);
    }
  }
}