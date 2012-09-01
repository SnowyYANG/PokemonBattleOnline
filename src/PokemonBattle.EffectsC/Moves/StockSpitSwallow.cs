using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves
{
  class Stockpile : StatusMoveE
  {
    public Stockpile(int id)
      : base(id)
    {
    }
    protected override bool NotFail(AtkContext atk)
    {
      return atk.Attacker.OnboardPokemon.GetCondition<int>("Stockpile") != 3;
    }
    protected override void Act(AtkContext atk)
    {
      var aer = atk.Attacker;
      int i = aer.OnboardPokemon.GetCondition<int>("Stockpile") + 1;
      aer.OnboardPokemon.SetCondition("Stockpile", i);
      aer.AddReportPm("EnStockpile", i);
      aer.ChangeLv7D(aer, false, 0, 1, 0, 1);
    }
  }
  class SpitUp : AttackMoveE
  {
    public SpitUp(int id)
      : base(id)
    {
    }
    protected override bool NotFail(AtkContext atk)
    {
      return atk.Attacker.OnboardPokemon.HasCondition("Stockpile");
    }
    protected override void CalculateBasePower(DefContext def)
    {
      def.BasePower = 100 * def.AtkContext.Attacker.OnboardPokemon.GetCondition<int>("Stockpile");
    }
    protected override void MoveEnding(AtkContext atk)
    {
      var aer = atk.Attacker;
      int i = aer.OnboardPokemon.GetCondition<int>("Stockpile");
      aer.ChangeLv7D(atk.Attacker, false, 0, -i, 0, -i);
      aer.OnboardPokemon.RemoveCondition("Stockpile");
      aer.AddReportPm("DeStockpile");
      base.MoveEnding(atk);
    }
  }
  class Swallow : StatusMoveE
  {
    public Swallow(int id)
      : base(id)
    {
    }
    protected override bool NotFail(AtkContext atk)
    {
      return atk.Attacker.OnboardPokemon.HasCondition("Stockpile");
    }
    protected override void Act(AtkContext atk)
    {
      base.Act(atk);
    }
  }
}
