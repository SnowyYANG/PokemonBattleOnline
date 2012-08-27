using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;
using LightStudio.PokemonBattle.Game.Host.Effects.Moves;
using LightStudio.PokemonBattle.Game.Host.Effects.Moves.Attack;
using LightStudio.PokemonBattle.Game.Host.Effects.Moves.Status;
using LightStudio.PokemonBattle.Game.Host.Effects.Abilities;
using LightStudio.PokemonBattle.Game.Host.Effects.Items;
using LightStudio.PokemonBattle.Game.Host.Effects.Triggers;
using GameService = LightStudio.PokemonBattle.Game.GameService;

namespace LightStudio.PokemonBattle.Game.Host.Effects
{
  public static class EffectsRegister
  {
    private static void A(IAbilityE ability)
    {
      EffectsService.Register(ability);
    }
    private static void I(IItemE item)
    {
      EffectsService.Register(item);
    }
    private static void M(IMoveE move)
    {
      EffectsService.Register(move);
    }
    
    public static void Register()
    {
      M(new GustTwister(16));  
      M(new Leap(19, CoordY.Air));//fly
      M(new SpRangeMove(57, CoordY.Water, true));
      M(new GrassKnot(67));
      M(new Counter(68, "Damage", 0x2000));
      M(new SolarBeam(76));
      M(new Thunder(87));
      M(new SpRangeMove(89, CoordY.Underground, true));//earthquake
      M(new SpRangeMove(90, CoordY.Underground));
      M(new Leap(91, CoordY.Underground));//dig
      M(new SkullBash(130));
      M(new Rest(156));
      M(new Substitute(164));
      M(new Flail(175));
      M(new Flail(179));
      M(new AHappy(216));
      M(new BHappy(218));
      M(new HiddenPower(237));
      M(new GustTwister(239));
      M(new Counter(243, "SpecialDamage", 0x1800));
      M(new SpRangeMove(250, CoordY.Water, true));//whirlpool
      M(new Spout(284));
      M(new Leap(291, CoordY.Water));//dives
      M(new Spout(323));
      M(new SpRangeMove(327, CoordY.Air));
      M(new Leap(340, CoordY.Air));//bounce
      M(new GyroBall(360));
      M(new Counter(368, "PhysicalDamage", 0x1800));
      M(new TrumpCard(376));
      M(new WringOut(378));
      M(new SLevel(386, 60));//punishment
      M(new GrassKnot(447));
      M(new WringOut(462));
      M(new Leap(467, CoordY.Another));//shadow
      M(new HeavySlam(484));
      M(new ElectroBall(486));
      M(new SLevel(500, 20));//stored power
      M(new Hex(506));
      M(new Acrobatics(512));
      M(new HeavySlam(535));
      M(new Thunder(542));//Hurricane

      M(new Curse(174));
      M(new HealBell(215, "HealBell"));
      M(new HealBell(312, "Aromatherapy"));

      A(new FlashFire(34));
      A(new Forewarn(37));
      A(new Frisk(39));
      A(new Illusion(56));

      I(new StickyBarb(65));
      I(new ReHurtBerry(191, MoveCategory.Physical));
      I(new ReHurtBerry(192, MoveCategory.Special));

      EffectsService.Register(new EndTurn());
      EffectsService.Register(new CanExecute());
      EffectsService.Register(new IsGroundAffectable());
    }
  }
}
