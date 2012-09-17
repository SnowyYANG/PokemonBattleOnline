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
    public override void Execute(PokemonProxy pm, GameEvents.UseMove eventForPP)
    {
      pm.BuildAtkContext(Move);
      int oldPP = pm.AtkContext.MoveProxy.PP;
      if (pm.Controller.OnboardPokemons.FirstOrDefault((p) => p.RaiseAbility(As.DAMP)) == null)
      {
        base.Execute(pm, eventForPP);
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
      if (Move.Class != MoveInnerClass.OHKO)
        foreach (DefContext d in atk.Targets) CalculateDamage(d);
      if (aer.UsingItem) aer.RaiseItem();
      aer.Pokemon.SetHp(0);
      aer.CheckFaint();
      Implement(atk.Targets.Where((d) => d.Defender.Pokemon.TeamId == atkTeam));
      Implement(atk.Targets.Where((d) => d.Defender.Pokemon.TeamId != atkTeam));
      if (atk.Type == BattleType.Fire)
        foreach (DefContext d in atk.Targets)
          if (d.Defender.State == PokemonState.Frozen) d.Defender.DeAbnormalState();

      PostEffect(atk);
    }
  }
}
