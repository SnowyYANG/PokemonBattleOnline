using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Game.Host.Triggers
{
  internal static class MoveCalculateType
  {
    public static void Execute(AtkContext atk)
    {
      switch (atk.Move.Id)
      {
        case Ms.STRUGGLE: //165
          atk.Type = BattleType.Invalid;
          break;
        case Ms.HIDDEN_POWER: //237
          HiddenPower(atk);
          break;
        case Ms.NATURAL_GIFT: //363
          {
            var i = ITs.BerryNumber(atk.Attacker.Pokemon.Item.Id);
            atk.Type = i < 36 ? BattleTypeHelper.GetItemType(i, 1) : i < 53 ? BattleTypeHelper.GetItemType(i, 36, false) : BattleTypeHelper.GetItemType(i, 50);
          }
          break;
        case Ms.JUDGMENT: //449
          if (atk.Attacker.Pokemon.Item != null)
          {
            var i = atk.Attacker.Pokemon.Item.Id;
            atk.Type = Is.FLAME_PLATE <= i && i <= Is.IRON_PLATE ? BattleTypeHelper.GetItemType(i, Is.FLAME_PLATE) : BattleType.Normal;
          }
          break;
        case Ms.TECHNO_BLAST: //546
          if (atk.Attacker.Pokemon.Item != null)
          {
            var i = atk.Attacker.Pokemon.Item.Id;
            atk.Type = i == Is.DOUSE_DRIVE ? BattleType.Water : i == Is.SHOCK_DRIVE ? BattleType.Electric : i == Is.BURN_DRIVE ? BattleType.Fire : i == Is.CHILL_DRIVE ? BattleType.Ice : BattleType.Normal;
          }
          break;
        default:
          atk.Type = atk.Attacker.Ability == As.NORMALIZE ? BattleType.Normal : atk.Move.Type;
          break;
      }
    }

    private static void HiddenPower(AtkContext atk)
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
