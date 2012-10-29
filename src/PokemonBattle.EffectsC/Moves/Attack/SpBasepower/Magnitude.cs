using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Attack
{
  class Magnitude : AttackMoveE
  {
    public Magnitude(int id)
      : base(id)
    {
    }
    public override AtkContext BuildAtkContext(PokemonProxy pm)
    {
      var atk = base.BuildAtkContext(pm);
      var random = pm.Controller.GetRandomInt(0, 99);
      if (random >= 95)
      {
        atk.SetCondition("Magnitude", 7);
        pm.Controller.ReportBuilder.Add("Magnitude", 10);
      }
      else
      {
        var a = random < 5 ? 0 : random < 16 ? 1 : random < 35 ? 2 : random < 65 ? 3 : random < 85 ? 4 : 5;
        atk.SetCondition("Magnitude", a);
        pm.Controller.ReportBuilder.Add("Magnitude", 4 + a);
      }
      return atk;
    }
    protected override void CalculateBasePower(DefContext def)
    {
      def.BasePower = 10 + 20 * def.AtkContext.GetCondition<int>("Magnitude");
    }
    protected override Modifier DamageFinalModifier(DefContext def)
    {
      return (ushort)(def.Defender.OnboardPokemon.CoordY == CoordY.Underground ? 0x2000 : 0x1000);
    }
  }
}
