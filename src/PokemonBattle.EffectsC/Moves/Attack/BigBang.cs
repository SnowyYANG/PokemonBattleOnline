using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.Host.Sp;
using As = LightStudio.PokemonBattle.Game.Host.Sp.Abilities;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Attack
{
  class BigBang : AttackMoveE
  {
    public BigBang(int id)
      : base(id)
    {
    }
    public override void Execute(PokemonProxy pm, GameEvents.UseMove eventForPP, AtkContextFlag flag)
    {
      pm.BuildAtkContext(Move);
      int oldPP = pm.AtkContext.MoveProxy.PP;
      if (pm.Controller.OnboardPokemons.FirstOrDefault((p) => p.RaiseAbility(As.DAMP)) == null)
      {
        base.Execute(pm, eventForPP, flag);
        if (pm.AtkContext.FailAll)
        {
          pm.Pokemon.SetHp(0);
          pm.CheckFaint();
        }
      }
      else pm.AddReportPm("FailSp", Move.Id);
      if (eventForPP != null) eventForPP.PP = oldPP - pm.AtkContext.MoveProxy.PP;
    }
    protected override void Act(AtkContext atk)
    {
      PokemonProxy aer = atk.Attacker;
      int atkTeam = aer.Pokemon.TeamId;
      CalculateDamages(atk);
      aer.Pokemon.SetHp(0);
      aer.Controller.ReportBuilder.Add(new GameEvents.HpChange(aer, null));
      aer.CheckFaint();
      Implement(atk.Targets.Where((d) => d.Defender.Pokemon.TeamId == atkTeam));
      Implement(atk.Targets.Where((d) => d.Defender.Pokemon.TeamId != atkTeam));
      if (atk.Type == BattleType.Fire)
        foreach (DefContext d in atk.Targets)
          if (d.Defender.State == PokemonState.FRZ) d.Defender.DeAbnormalState();

      PostEffect(atk);
    }
  }
}
