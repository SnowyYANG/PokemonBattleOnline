using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Attack
{
  class HiddenPower: AttackMoveE
  {
    public HiddenPower(int MoveId)
      : base(MoveId)
    { }

    protected override void  CalculateBasePower(DefContext def)
    {
      var iv = def.AtkContext.Attacker.Pokemon.Iv;
      int pI = (iv.Hp & 2) >> 1;
      pI |= (iv.Atk & 2);
      pI |= (iv.Def & 2) << 1;
      pI |= (iv.Speed & 2) << 2;
      pI |= (iv.SpAtk & 2) << 3;
      pI |= (iv.SpDef & 2) << 4;
      def.BasePower = (int)(pI * 40 / 63 + 30);
    }

    protected override void CalculateType(AtkContext atk)
    {
      var iv = atk.Attacker.Pokemon.Iv;
      int pI = iv.Hp & 1;
      pI |= (iv.Atk & 1) << 1;
      pI |= (iv.Def & 1) << 2;
      pI |= (iv.Speed & 1) << 3;
      pI |= (iv.SpAtk & 1) << 4;
      pI |= (iv.SpDef & 1) << 5;
      atk.Type = (BattleType)(pI * 15 / 63 + 2);
    }
  }
}
