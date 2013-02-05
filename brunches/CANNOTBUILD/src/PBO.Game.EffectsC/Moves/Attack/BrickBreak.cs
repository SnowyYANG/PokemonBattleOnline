using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Game.Host.Effects.Moves.Attack
{
  class BrickBreak : AttackMoveE
  {
    public BrickBreak(int id)
      : base(id)
    {
    }
    protected override void CalculateDamages(AtkContext atk)
    {
      bool ls = false, r = false;
      var f = atk.Target.Defender.Tile.Field;
      if (f.Team != atk.Attacker.Pokemon.TeamId)
      {
        ls = f.RemoveCondition("LightScreen");
        r = f.RemoveCondition("Reflect");
      }
      base.CalculateDamages(atk);
      if (ls) atk.Controller.ReportBuilder.Add("DeLightScreen", f.Team);
      if (r) atk.Controller.ReportBuilder.Add("DeReflect", f.Team);
    }
  }
}
