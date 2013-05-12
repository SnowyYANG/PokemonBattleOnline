using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Status
{
  class Curse : StatusMoveE
  {
    public Curse(int id)
      : base(id)
    {
    }

    protected override void Act(AtkContext atk)
    {
      var aer = atk.Attacker;
      if (aer.OnboardPokemon.HasType(BattleType.Ghost))
      {
        if (atk.Target.Defender.OnboardPokemon.AddCondition("Curse"))
        {
          aer.Pokemon.SetHp(aer.Hp - (aer.Pokemon.Hp.Origin >> 1));
          atk.Controller.ReportBuilder.Add(new GameEvents.HpChange(aer, "EnCurse", atk.Target.Defender.Id));
          aer.CheckFaint();
        }
        else atk.FailAll();
      }
      else aer.ChangeLv7D(aer, true, 1, 1, 0, 0, -1);
    }
  }
}
