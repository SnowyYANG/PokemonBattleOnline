using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Game.Host.Effects.Moves.Attack
{
  class TripleKick : AttackMoveE
  {
    public TripleKick(int id)
      : base(id)
    {
    }
    protected override void Act(AtkContext atk)
    {
      PokemonProxy aer = atk.Attacker;

      //生成攻击次数
      int times = 3;

      int atkTeam = aer.Pokemon.TeamId;
      int hits = 0;
      do
      {
        hits++;
        atk.Target.BasePower += 10;
        CalculateDamages(atk);
        Implement(atk.Targets);
        FilterDefContext(atk);
      }
      while (atk.Target != null && hits < times && atk.Target.Defender.Hp != 0 && aer.Hp != 0 && aer.State != PokemonState.FRZ && aer.State != PokemonState.SLP);

      atk.Controller.ReportBuilder.Add("Hits", hits);

      FinalEffect(atk);
    }
    protected override void CalculateBasePower(DefContext def)
    {
    }
  }
}
