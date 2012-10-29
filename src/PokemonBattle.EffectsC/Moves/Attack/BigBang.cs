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
    private void Suicide(PokemonProxy pm)
    {
      pm.Pokemon.SetHp(0);
      pm.Controller.ReportBuilder.Add(new GameEvents.HpChange(pm, null));
      pm.CheckFaint();
    }
    public override void Execute(AtkContext atk)
    {
      if (atk.Controller.OnboardPokemons.FirstOrDefault((p) => p.RaiseAbility(As.DAMP)) == null)
      {
        base.Execute(atk);
        if (atk.FailAll) Suicide(atk.Attacker);
      }
      else atk.Attacker.AddReportPm("FailSp", Move.Id);
    }
    protected override void Act(AtkContext atk)
    {
      PokemonProxy aer = atk.Attacker;
      int atkTeam = aer.Pokemon.TeamId;
      CalculateDamages(atk);
      Suicide(aer);
      Implement(atk.Targets.Where((d) => d.Defender.Pokemon.TeamId == atkTeam));
      Implement(atk.Targets.Where((d) => d.Defender.Pokemon.TeamId != atkTeam));
      if (atk.Type == BattleType.Fire)
        foreach (DefContext d in atk.Targets)
          if (d.Defender.State == PokemonState.FRZ) d.Defender.DeAbnormalState();
      PostEffect(atk);
    }
  }
}
