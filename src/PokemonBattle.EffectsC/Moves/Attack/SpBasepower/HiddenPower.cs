using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves
{
  class HiddenPower: AttackMoveE
  {
    public HiddenPower(int MoveId)
      : base(MoveId)
    { }

    protected override void  CalculateBasePower(DefContext def)
    {
      int pI = 0;
      if (def.AtkContext.Attacker.Pokemon.Iv.Hp % 4 >= 2)
        pI += 1;
      if (def.AtkContext.Attacker.Pokemon.Iv.Atk % 4 >= 2)
        pI += 2;
      if (def.AtkContext.Attacker.Pokemon.Iv.Def % 4 >= 2)
        pI += 4;
      if (def.AtkContext.Attacker.Pokemon.Iv.Speed % 4 >= 2)
        pI += 8;
      if (def.AtkContext.Attacker.Pokemon.Iv.SpAtk % 4 >= 2)
        pI += 16;
      if (def.AtkContext.Attacker.Pokemon.Iv.SpDef % 4 >= 2)
        pI += 32;
      def.BasePower = (int)(pI * 40 / 63 + 30);
    }

    protected override void CalculateType(AtkContext atk)
    {
      int pI = 0;
      if (atk.Attacker.Pokemon.Iv.Hp % 2 == 1)
        pI += 1;
      if (atk.Attacker.Pokemon.Iv.Atk % 2 == 1)
        pI += 2;
      if (atk.Attacker.Pokemon.Iv.Def % 2== 1)
        pI += 4;
      if (atk.Attacker.Pokemon.Iv.Speed % 2 == 1)
        pI += 8;
      if (atk.Attacker.Pokemon.Iv.SpAtk % 2 == 1)
        pI += 16;
      if (atk.Attacker.Pokemon.Iv.SpDef % 2 == 1)
        pI += 32;
      pI=(int)(pI*15/63);
      atk.Type  = (BattleType)(pI + 2);
    }
  }
}
