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
    private static void A(AbilityE ability)
    {
      EffectsService.Register(ability);
    }
    private static void I(ItemE item)
    {
      EffectsService.Register(item);
    }
    private static void M(MoveE move)
    {
      EffectsService.Register(move);
    }
    
    public static void Register()
    {
      M(new GustTwister(16));  
      M(new Leap(19, CoordY.Air));//fly
      M(new JumpKick(26));
      M(new RandomMTA(37));//thrash
      M(new SpRangeMove(57, CoordY.Water, true));
      M(new GrassKnot(67));
      M(new Counter(68, "PhysicalDamage", 0x2000));
      M(new SolarBeam(76));
      M(new RandomMTA(80));//petal dance
      M(new Thunder(87));
      M(new SpRangeMove(89, CoordY.Underground, true));//earthquake
      M(new SpRangeMove(90, CoordY.Underground));
      M(new Leap(91, CoordY.Underground));//dig
      M(new Bad(100));
      M(new Mimic(102));
      M(new Bide(117));
      M(new Metronome(118));
      M(new MirrorMove(119));
      M(new BigBang(120));
      M(new SkullBash(130));
      M(new JumpKick(136));
      M(new Transform(144));
      M(new Splash(150));
      M(new BigBang(153));
      M(new Rest(156));
      M(new Conversion(160));
      M(new TriAttack(161));
      M(new Substitute(164));
      M(new Bad(166));
      M(new TripleKick(167));
      M(new Thief(168));
      M(new Curse(174));
      M(new Flail(175));
      M(new Conversion2(176));
      M(new Flail(179)); //reversal
      M(new Spite(180));
      M(new RandomMTA(200));//outrage
      M(new Attack5Turns(205));//rollout
      M(new FuryCutter(210));
      M(new SleepTalk(214));
      M(new HealBell(215, "HealBell"));
      M(new Happiness(216, false));
      M(new Present(217));
      M(new Happiness(218, true));
      M(new PainSplit(220));
      M(new Magnitude(222));
      M(new BatonPass(226));
      M(new Encore(227));
      M(new RapidSpin(229));
      M(new HiddenPower(237));
      M(new GustTwister(239));
      M(new Counter(243, "SpecialDamage", 0x2000));
      M(new FSDD(248));
      M(new SpRangeMove(250, CoordY.Water, true));//whirlpool
      M(new BeatUp(251));
      M(new Uproar(253));
      M(new Stockpile(254));
      M(new SpitUp(255));
      M(new Swallow(256));
      M(new NaturePower(267));
      M(new Trick(271));
      M(new RolePlay(272));
      M(new Assist(274));
      M(new Recycle(278));
      M(new Revenge(279));
      M(new BrickBreak(280));
      M(new RemoveItem(282, false, "KnockOff"));
      M(new Spout(284));
      M(new SkillSwap(285));
      M(new Snatch(289));
      M(new SecretPower(290));
      M(new Leap(291, CoordY.Water));//dives
      M(new Attack5Turns(301));//ice ball
      M(new HealBell(312, "Aromatherapy"));
      M(new Spout(323));
      M(new SpRangeMove(327, CoordY.Air));
      M(new Leap(340, CoordY.Air));//bounce
      M(new Thief(343));//covet
      M(new FSDD(353));
      M(new Gravity(356));
      M(new GyroBall(360));
      M(new Feint(364));
      M(new NaturalGift(363));
      M(new EatDefenderBerry(365));
      M(new Acupressure(367));
      M(new Counter(368, "Damage", 0x1800));
      M(new Fling(374));
      M(new TrumpCard(376));
      M(new WringOut(378));
      M(new MeFirst(382));
      M(new Copycat(383));
      M(new SLevel(386, 60));//punishment
      M(new SetAbility(388, 15));//worry seed
      M(new Trick(415)); //switcheroo
      M(new Revenge(419));//avalanche
      M(new GrassKnot(447));
      M(new Chatter(448));
      M(new EatDefenderBerry(450));
      M(new WringOut(462));
      M(new Leap(467, CoordY.Another));//shadow
      M(new WonderRoom(472));
      M(new Autotomize(475));
      M(new SmackDown(479));
      M(new HeavySlam(484));
      M(new ElectroBall(486));
      M(new SetAbility(493, 86));//simple beam
      M(new Entrainment(494));
      M(new EchoedVoice(497));
      M(new SLevel(500, 20));//stored power
      M(new Hex(506));
      M(new SkyDrop(507));
      M(new AttackAndForceSwitch(509));//circle throw
      M(new RemoveItem(510, true, "Incinerate"));
      M(new Acrobatics(512));
      M(new AttackAndForceSwitch(525));//dragon tail
      M(new HeavySlam(535));
      M(new Thunder(542));//hurricane
      M(new RelicSong(547));

      A(new AirLock(13));//cloud nine
      A(new Forecast(59));
      A(new AirLock(76));
      A(new FlashFire(18));
      A(new Forewarn(108));
      A(new Frisk(119));
      A(new FlowerGift(122));
      A(new Illusion(149));
      A(new Imposter(150));
      A(new ZenMode(161));

      I(new MentalHerb(8));
      I(new StickyBarb(65));
      I(new Eviolite(102));
      I(new ReHurtBerry(191, MoveCategory.Physical));
      I(new ReHurtBerry(192, MoveCategory.Special));
      I(new TastyBerry(139));
      I(new TastyBerry(140));
      I(new TastyBerry(141));
      I(new TastyBerry(142));
      I(new TastyBerry(143));

      EffectsService.Register(new EndTurn());
      EffectsService.Register(new CanExecute());
      EffectsService.Register(new IsGroundAffectable());
    }
  }
}
