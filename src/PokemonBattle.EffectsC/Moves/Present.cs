using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves
{
  class Present : AttackMoveE
  {
    public Present(int id)
      : base(id)
    {
    }
    public override AtkContext BuildAtkContext(PokemonProxy pm)
    {
      var atk = base.BuildAtkContext(pm);
      var random = pm.Controller.GetRandomInt(0, 99);
      atk.SetCondition("Present", random < 20 ? 0 : random < 60 ? 40 : random < 90 ? 80 : 100);
      return atk;
    }
    protected override void Act(AtkContext atk)
    {
      if (atk.GetCondition<int>("Present") == 0) atk.Target.Defender.HpRecoverByOneNth(4, true);
      else base.Act(atk);
    }
    protected override void CalculateBasePower(DefContext def)
    {
      def.BasePower = def.AtkContext.GetCondition<int>("Present");
    }
  }
}
