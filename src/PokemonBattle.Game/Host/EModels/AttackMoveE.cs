using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Interactive;
using LightStudio.PokemonBattle.Interactive.GameEvents;
using LightStudio.PokemonBattle.Game.Sp;

namespace LightStudio.PokemonBattle.Game
{
  public class AttackMoveE : MoveE
  {
    public AttackMoveE(int moveId)
      : base(moveId)
    {
    }

    private void Implement(IEnumerable<DefContext> defs)
    {
      if (defs.Count() == 0) return;
      DefContext def = defs.First();
      if (Move.Class == MoveInnerClass.OHKO && !Sp.Conditions.Substitute.OHKO(def))
      {
        def.Damage = def.Defender.Hp;
        MoveHurts e = new MoveHurts();
        def.Defender.MoveHurt(def);
        e.SetHurt(defs);
        def.Defender.Controller.ReportBuilder.Add("OHKO");
        ImplementEffect(def);
      }
      else
      {
        AtkContext atk = def.AtkContext;
        PokemonProxy a = atk.Attacker;
        int m = 0;
        do
        {
          bool allSub = true;
          foreach (DefContext d in defs)
            Calculate(d);
          if (!Move.AdvancedFlags.IgnoreSubstitute)
          {
            foreach (DefContext d in defs)
              if (!Sp.Conditions.Substitute.Hurt(d)) allSub = false;
          }
          if (!allSub)
          {
            MoveHurts e = new MoveHurts();
            a.Controller.ReportBuilder.Add(e);
            foreach (DefContext d in defs)
              d.Defender.MoveHurt(d);
            e.SetHurt(defs);
          }

          if (Move.HurtPercentage > 0)
            a.DamagePercentage(def, Move.HurtPercentage);
          if (Move.Class == MoveInnerClass.AttackWithSelfLv7DChange)
            a.ChangeLv7D(atk);

          foreach (DefContext d in defs)
            if (!d.HitSubstitute) ImplementEffect(d);
          m++;
          if (def.Defender.Hp == 0 || a.Hp == 0 || a.State == PokemonState.Sleeping || a.State == PokemonState.Frozen)
            break;
        } while (m < atk.Times);
        if (Move.MaxTimes > 1)
          a.Controller.ReportBuilder.Add("Hits", m.ToString());

        if (a.Hp > 0)
        {
          if (Move.HurtPercentage < 0) a.DamagePercentage(atk.Target, Move.HurtPercentage);
          else if (Move.MaxHpPercentage != 0) //拼命专用
          {
            a.Hp -= a.Pokemon.Hp.Origin * Move.MaxHpPercentage;
            a.Controller.ReportBuilder.Add(new HpChange(a, "ReHurt"));
          }
        }
      }// else OHKO
    }
    public override void Execute(PokemonProxy pm)
    {
      if (pm.AtkContext == null) pm.BuildAtkContext(Move);
      if (Move.AdvancedFlags.PrepareOneTurn && PrepareOneTurn(pm) && !Sp.Items.CheckPowerHerb(pm))
        return;
      AtkContext atk = pm.AtkContext;
      pm.OnboardPokemon.CoordY = CoordY.Plate;
      if (!Abilities.CalculateType(atk)) CalculateType(atk);
      var targets = GetRangeTiles(atk).ToArray();
      if (targets.Length == 0)
      {
        pm.Controller.ReportBuilder.Add("Fail");
        goto DONE;
      }
      else
      {
        BuildDefContexts(atk, targets);
        if (atk.Targets == null) goto DONE;
      }

      if (Move.Class != MoveInnerClass.OHKO) Calculate(atk);

      int atkTeam = atk.Attacker.Pokemon.TeamId;
      Implement(atk.Targets.Where((d) => d.Defender.Pokemon.TeamId == atkTeam));
      Implement(atk.Targets.Where((d) => d.Defender.Pokemon.TeamId != atkTeam));

      if (!atk.SheerForceActive) PostEffect(atk);
      EndAttack(atk);
#warning 逆鳞
    
    DONE:
      pm.Action = PokemonAction.Done;
    }

    protected virtual bool PrepareOneTurn(PokemonProxy pm)
    {
      if (pm.Action == PokemonAction.MoveAttached)
      {
        pm.AddReportPm("Prepare" + Move.Id.ToString());
        pm.Action = PokemonAction.Moving;
        return true;
      }
      return false;
    }
    protected virtual void CalculateBasePower(DefContext def)
    {
      def.BasePower = Move.Power;
    }
    protected virtual Modifier GetPowerModifier(DefContext def)
    {
      return 0x1000;
    }
    protected virtual void Calculate(AtkContext atk) //固定伤害、大爆炸、佯攻override
    {
      PokemonProxy pm = atk.Attacker;

      {
        atk.Attacker.Item.CalculatingPowerModifier(atk);
        if (atk.Type == BattleType.Fire)
        {
          if (pm.Controller.Board.HasCondition("WaterSport")) atk.PowerModifier_Board = 0x800;
        }
        else if (atk.Type == BattleType.Electric && pm.Controller.Board.HasCondition("MudSport"))
          atk.PowerModifier_Board = 0x800;
      }

      {
        OnboardPokemon p;
        if (Move.FoulPlay()) p = atk.Target.Defender.OnboardPokemon;
        else p = pm.OnboardPokemon;
        StatType st = Move.Category == MoveCategory.Physical ? StatType.Atk : StatType.SpAtk;
        atk.AtkRaw = p.Static.GetStat(st);
        atk.AtkLv = p.Lv5D.GetStat(st);
        atk.AtkModifier *= pm.Ability.ADSModifier(pm, st);
        Abilities.FlowerGift(atk);
        atk.AtkModifier *= pm.Item.ADSModifier(pm, st);
      }

      atk.CTLv = Move.CtLv;
      if (atk.CTLv < 5)
      {
        if (pm.OnboardPokemon.HasCondition("FocusEnergy")) atk.CTLv += 2;
        if (pm.Ability.SuperLuck()) atk.CTLv++;
        atk.CTLv += pm.Item.GetCtLvRevise(pm);
        if (atk.CTLv > 4) atk.CTLv = 4;
      }

      //生成攻击次数
      if (!Sp.Abilities.CheckSkillLink(atk) && Move.MinTimes != Move.MaxTimes)
        atk.Times = TIMES25[atk.Controller.GetRandomInt(0, 7)];
      else atk.Times = Move.MinTimes;
      if (atk.Times == 0) atk.Times = 1;

      //天气
      Weather w = atk.Controller.GetAvailableWeather();
      BattleType type = atk.Type;
      if (w == Weather.IntenseSunlight)
      {
        if (type == BattleType.Water) atk.WeatherModifier = 0x800;
        else if (type == BattleType.Fire) atk.WeatherModifier = 0x1800;
      }
      else if (w == Weather.HeavyRain)
      {
        if (type == BattleType.Water) atk.WeatherModifier = 0x1800;
        else if (type == BattleType.Fire) atk.WeatherModifier = 0x800;
      }
      //宝石、节拍器、生命玉
      //本属性修正
      if (atk.Attacker.OnboardPokemon.HasType(atk.Type))
        atk.STAB = (ushort)(atk.Attacker.Ability.Adaptability() ? 0x2000 : 0x1800);
    }
    protected virtual void Calculate(DefContext def)
    {
      AtkContext atk = def.AtkContext;
      PokemonProxy aer = atk.Attacker;
      Controller c = aer.Controller;
      int defRaw = 0;

      if (!(c.Board[def.Defender.Pokemon.TeamId].HasCondition("LuckyChant") || def.Ability.CannotBeCted()))
        if (Move.CtLv > 5) def.IsCt = true;
        else def.IsCt = c.OneNth(LV_CT[atk.CTLv]);

      def.Damage = aer.Pokemon.Lv * 2 / 5 + 2;
      {
        CalculateBasePower(def);
        Modifier m = atk.Attacker.Ability.PowerModifier(def);
        m *= Abilities.PowerModifier(def);
        m *= atk.PowerModifier_Item;
        m *= GetPowerModifier(def);
        m *= atk.PowerModifier_Board;
        def.Damage *= (def.BasePower * m);
      }
      {
        int atkLv = 0;
        if (!(def.Ability.Unaware() || (def.IsCt && atk.AtkLv < 0)))
          atkLv = atk.AtkLv;
        int a = OnboardPokemon.Get5D(atk.AtkRaw, atkLv);
        Modifier m = Abilities.ThickFat(def);
        def.Damage *= (a * Abilities.Hustle(atk)) * (m * atk.AtkModifier); //计算顺序有异，但介于具体数值，精度无损
      }
      {
        StatType st = Move.Category == MoveCategory.Physical ? StatType.Def : StatType.SpDef;
        defRaw = def.Defender.OnboardPokemon.Static.GetStat(st);
        int defLv = 0;
        if (!(aer.Ability.Unaware() || Move.ChipAway())) defLv = def.Defender.OnboardPokemon.Lv5D.GetStat(st);
        if (def.IsCt && defLv > 0) defLv = 0;
        int d = OnboardPokemon.Get5D(defRaw, defLv);
        Modifier m = 0x1000;
        if (!aer.Ability.IgnoreDefenderAbility())
        {
          m = def.Ability.ADSModifier(def.Defender, st); //Marvel Scale only
          m *= Abilities.FlowerGift(def);
        }
        m *= def.Defender.Item.ADSModifier(def.Defender, st);
        def.Damage /= (d * m);
      }
      def.Damage /= 50;
      def.Damage += 2;
      //1.Apply the multi-target modifier
      if (atk.MultiTargets) def.ModifyDamage(0xC00);
      //2.Apply the weather modifier
      def.Damage *= atk.WeatherModifier;
      //3.In case of a critical hit, double the value
      if (def.IsCt) def.Damage <<= 1;
      //4.Alter with a random factor
      def.Damage *= aer.Controller.GetRandomInt(85, 100);
      def.Damage /= 100;
      //5.Apply STAB modifier
      def.Damage *= atk.STAB;
      //6.Alter with type effectiveness
      if (def.EffectRevise > 0) def.Damage <<= def.EffectRevise;
      else if (def.EffectRevise < 0) def.Damage >>= -def.EffectRevise;
      //7.Alter with user's burn
      if (Move.Category == MoveCategory.Physical && aer.State == PokemonState.Burned && !aer.Ability.Guts())
        def.Damage >>= 1;
      //8.Make sure damage is at least 1
      if (def.Damage < 1) def.Damage = 1;
      //9.Apply the final modifier
      {
        Modifier m = Sp.Conditions.LightScreenReflect.DamageFinalModifier(def);
        //If the target's ability is Multiscale and the target is at full health.
        m *= Abilities.Multiscale(def);
        //If the user's ability is Tinted Lens and the move wasn't very effective.
        m *= Abilities.TintedLens(def);
        //If one of the target's allies' ability is Friend Guard.
        m *= Abilities.FriendGuard(def);
        //If user has ability Sniper and move was a critical hit.
        m *= Abilities.Sniper(def);
        //If the target's ability is Solid Rock or Filter and the move was super effective.
        m *= Abilities.FilterSolidRock(def);
        //metronome expertbelt lifeorb
        m *= Items.DamageFinalModifier(def);
        //If the target is holding a damage lowering berry of the attack's type.
        m *= def.Defender.Item.DamageFinalModifier(def);
        
        def.Damage *= m;
      }
    }
    protected virtual void ImplementEffect(DefContext def)
    {
      AtkContext atk = def.AtkContext;
      PokemonProxy d = def.Defender;
      bool sheerForce = def.AtkContext.SheerForceActive;
      if (d.Hp > 0 && !sheerForce)
      {
        switch (Move.Class)
        {
          case MoveInnerClass.AttackWithTargetLv7DChange:
            d.ChangeLv7D(atk);
            break;
          case MoveInnerClass.AttackWithState:
            if (!Moves.CheckTriAttack(def) && atk.RandomHappen(Move.Attachment.Probability))
              d.AddState(atk);
            break;
        }
        if (def.AtkContext.RandomHappen(Move.FlinchProbability, true) || Items.CanAttackFlinch(def))
          d.OnboardPokemon.SetTurnCondition("Flinch");
        //恶臭/毒手
      }
      d.Ability.Attacked(def);//此时破格不能无视
      d.Item.Attacked(def);
      atk.Attacker.CheckFaint();
      d.CheckFaint();
      Abilities.CheckMoxie(def);
      if (Move.MaxTimes > 1) d.Item.HpChanged(d);
    }
    protected virtual void PostEffect(AtkContext atk)
    {
      foreach (DefContext d in atk.Targets)
        Abilities.CheckColorChange(d);
      //红牌、逃生按钮
      Items.AttackPostEffect(atk);
    }
    protected virtual void EndAttack(AtkContext atk)
    {
    }
  }
}