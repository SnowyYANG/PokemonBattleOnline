using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.GameEvents;
using LightStudio.PokemonBattle.Game.Host.Sp;

namespace LightStudio.PokemonBattle.Game.Host
{
  public class AttackMoveE : MoveE
  {
    protected static readonly int[] TIMES25 = new int[8] { 2, 2, 2, 3, 3, 3, 4, 5 };
    protected static readonly int[] LV_CT = { 16, 8, 4, 3, 2, 0 };

    public static void DamagePercentage(DefContext def, int percentage)
    {
      var aer = def.AtkContext.Attacker;
      var ability = aer.Ability;
      int v = def.Damage * percentage / 100;
      if (percentage > 0)
      {
        if (aer.Item.BigRoot()) v = (int)(v * 1.3);
        if (!ability.MagicGuard() && def.Defender.RaiseAbility(Abilities.LIQUID_OOZE)) aer.EffectHurt(v);
        else aer.HpRecover(v, false);
      }
      else //ReHurt
      {
        if (ability.RockHead() || ability.MagicGuard()) return;
        aer.Pokemon.SetHp(aer.Hp + v);
        aer.Controller.ReportBuilder.Add(new GameEvents.HpChange(aer, "ReHurt"));
      }
    }
    
    public AttackMoveE(int moveId)
      : base(moveId)
    {
    }

    public override void Execute(PokemonProxy pm, UseMove eventForPP)
    {
      if (pm.AtkContext == null)
      {
        pm.BuildAtkContext(Move);
        Moves.MultiTurnAttack(pm.AtkContext);
      }
      if (PrepareOneTurn(pm) && !Sp.Items.PowerHerb(pm)) return;
      base.Execute(pm, eventForPP);
    }
    protected override void Act(AtkContext atk)
    {
      PokemonProxy aer = atk.Attacker;
      
      //生成攻击次数
      int times = Move.MinTimes == Move.MaxTimes || atk.Attacker.Ability.SkillLink() ? Move.MaxTimes : TIMES25[atk.Controller.GetRandomInt(0, 7)];
      
      int atkTeam = aer.Pokemon.TeamId;
      int hits = 0;
      do
      {
        hits++;
        CalculateDamages(atk);
        Implement(atk.Targets.Where((d) => d.Defender.Pokemon.TeamId == atkTeam));
        Implement(atk.Targets.Where((d) => d.Defender.Pokemon.TeamId != atkTeam));
      }
      while (hits < times && atk.Target.Defender.Hp != 0 && aer.Hp != 0 && aer.State != PokemonState.FRZ && aer.State != PokemonState.SLP && Moves.TripleKick(atk));
      
      if (Move.MaxTimes > 1) atk.Controller.ReportBuilder.Add("Hits", hits);
      if (atk.Type == BattleType.Fire)
        foreach (DefContext d in atk.Targets)
          if (d.Defender.State == PokemonState.FRZ) d.Defender.DeAbnormalState();
      
      if (!(atk.Move.HasProbabilitiedAdditonalEffects() && aer.Ability.SheerForce())) PostEffect(atk);
    }

    protected virtual void Implement(IEnumerable<DefContext> defs)
    {
      if (defs.Count() == 0) return;
      DefContext def = defs.First();
      if (Move.Class == MoveInnerClass.OHKO)
      {
        if (!Sp.Conditions.Substitute.OHKO(def))
        {
          def.Damage = def.Defender.Hp;
          MoveHurts e = new MoveHurts();
          def.Defender.MoveHurt(def);
          e.SetHurt(defs);
          def.Defender.Controller.ReportBuilder.Add("OHKO");
          ImplementEffect(def);
        }
      }
      else
      {
        AtkContext atk = def.AtkContext;
        PokemonProxy a = atk.Attacker;
        bool allSub = true;
        if (!Move.AdvancedFlags.IgnoreSubstitute)
        {
          foreach (DefContext d in defs)
            if (!Sp.Conditions.Substitute.Hurt(d)) allSub = false;
        }
        if (!allSub)
        {
          foreach (DefContext d in defs)
            if (d.Defender.UsingItem) d.Defender.RaiseItem();
          MoveHurts e = new MoveHurts();
          a.Controller.ReportBuilder.Add(e);
          foreach (DefContext d in defs)
          {
            d.Defender.MoveHurt(d);
            atk.TotalDamage += d.Damage;
          }
          e.SetHurt(defs);
        }

        if (Move.HurtPercentage > 0) DamagePercentage(def, Move.HurtPercentage);
        if (Move.Class == MoveInnerClass.AttackWithSelfLv7DChange && !(Move.HasProbabilitiedAdditonalEffects() && a.Ability.SheerForce())) a.ChangeLv7D(atk);

        foreach (DefContext d in defs) if (!d.HitSubstitute) ImplementEffect(d);

        if (a.Hp > 0)
        {
          if (Move.HurtPercentage < 0) DamagePercentage(def, Move.HurtPercentage);
          else if (Move.MaxHpPercentage < 0) //拼命专用
          {
            a.Pokemon.SetHp(a.Hp + a.Pokemon.Hp.Origin * Move.MaxHpPercentage / 100);
            a.Controller.ReportBuilder.Add(new HpChange(a, "ReHurt"));
          }
          a.CheckFaint();
        }
      }// OHKO else
    }
    protected virtual void CalculateBasePower(DefContext def)
    {
      def.BasePower = Move.Power;
    }
    protected virtual Modifier PowerModifier(DefContext def)
    { return 0x1000; }
    protected virtual void CalculateEffectRevise(DefContext def)
    {
      BattleType a = def.AtkContext.Type;
      OnboardPokemon der = def.Defender.OnboardPokemon;
      def.EffectRevise = a == BattleType.Ground && def.Defender.Item.IronBall() && der.HasType(BattleType.Flying)? 0 : a.EffectRevise(der.Type1, der.Type2);
    }
    protected virtual void CalculateDamages(AtkContext atk)
    {
      if (Move.Class != MoveInnerClass.OHKO)
      {
        Items.CheckGem(atk);
        foreach (DefContext d in atk.Targets) CalculateDamage(d);
      }
    }
    protected virtual void CalculateDamage(DefContext def)
    {
      AtkContext atk = def.AtkContext;
      PokemonProxy aer = atk.Attacker;
      Controller c = aer.Controller;

      atk.CTLv = Move.CtLv;
      if (atk.CTLv < 5)
      {
        if (aer.OnboardPokemon.HasCondition("FocusEnergy")) atk.CTLv += 2;
        if (aer.Ability.SuperLuck()) atk.CTLv++;
        atk.CTLv += aer.Item.CtLvRevise(aer);
        if (atk.CTLv > 4) atk.CTLv = 4;
      }
      if (!(c.Board[def.Defender.Pokemon.TeamId].HasCondition("LuckyChant") || def.Ability.CannotBeCted()))
        if (Move.CtLv > 5) def.IsCt = true;
        else def.IsCt = c.OneNth(LV_CT[atk.CTLv]);

      def.Damage = aer.Pokemon.Lv * 2 / 5 + 2;
      {
        CalculateBasePower(def);
        def.Damage *= def.BasePower * Triggers.PowerModifier(def, PowerModifier);
      }
      {
        StatType st = Move.Category == MoveCategory.Physical ? StatType.Atk : StatType.SpAtk;
        int a;
        {
          OnboardPokemon p;
          StatType s;
          if (Move.UseDefenderAtk())
          {
            p = def.Defender.OnboardPokemon;
            s = StatType.Atk;
          }
          else
          {
            p = atk.Attacker.OnboardPokemon;
            s = st;
          }
          a = p.FiveD.GetStat(st);
          if (!def.Ability.Unaware())
          {
            int atkLv = p.Lv5D.GetStat(st);
            if (!(def.IsCt && atkLv < 0)) a = OnboardPokemon.Get5D(a, atkLv);
          }
        }
        a *= Abilities.Hustle(atk);
        Modifier m = Abilities.ThickFat(def);
        m *= aer.Ability.ADSModifier(aer, st);
        m *= Abilities.FlowerGift(atk);
        m *= aer.Item.ADSModifier(aer, st);
        def.Damage *= a * m;
      }
      {
        StatType st = Move.Category == MoveCategory.Physical ? StatType.Def : StatType.SpDef;
        int defRaw = def.Defender.OnboardPokemon.FiveD.GetStat(st);
        int defLv = 0;
        if (!(aer.Ability.Unaware() || Move.IgnoreDefenderLv7D())) defLv = def.Defender.OnboardPokemon.Lv5D.GetStat(st);
        if (def.IsCt && defLv > 0) defLv = 0;
        int d = OnboardPokemon.Get5D(defRaw, defLv);
        Modifier m = 0x1000;
        if (!aer.Ability.IgnoreDefenderAbility())
        {
          m = def.Ability.ADSModifier(def.Defender, st); //Marvel Scale only
          m *= Abilities.FlowerGift(def);
        }
        m *= def.Defender.Item.ADSModifier(def.Defender, st);
        def.Damage /= d * m;
      }
      def.Damage /= 50;
      def.Damage += 2;
      //1.Apply the multi-target modifier
      if (atk.MultiTargets) def.ModifyDamage(0xC00);
      //2.Apply the weather modifier
      {
        Weather w = c.Weather;
        BattleType type = atk.Type;
        if (w == Weather.IntenseSunlight)
        {
          if (type == BattleType.Water) def.ModifyDamage(0x800);
          else if (type == BattleType.Fire) def.ModifyDamage(0x1800);
        }
        else if (w == Weather.HeavyRain)
        {
          if (type == BattleType.Water) def.ModifyDamage(0x1800);
          else if (type == BattleType.Fire) def.ModifyDamage(0x800);
        }
      }
      //3.In case of a critical hit, double the value
      if (def.IsCt) def.Damage <<= 1;
      //4.Alter with a random factor
      def.Damage *= aer.Controller.GetRandomInt(85, 100);
      def.Damage /= 100;
      //5.Apply STAB modifier
      if (atk.Attacker.OnboardPokemon.HasType(atk.Type))
        def.ModifyDamage((ushort)(atk.Attacker.Ability.Adaptability() ? 0x2000 : 0x1800));
      //6.Alter with type effectiveness
      CalculateEffectRevise(def);
      if (def.EffectRevise > 0) def.Damage <<= def.EffectRevise;
      else if (def.EffectRevise < 0) def.Damage >>= -def.EffectRevise;
      //7.Alter with user's burn
      if (Move.Category == MoveCategory.Physical && aer.State == PokemonState.BRN && !aer.Ability.Guts())
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

        def.Damage *= (m * DamageFinalModifier(def));
      }
    }
    protected virtual Modifier DamageFinalModifier(DefContext def)
    { return 0x1000; }
    protected virtual void ImplementEffect(DefContext def)
    {
      AtkContext atk = def.AtkContext;
      PokemonProxy d = def.Defender;
      if (d.Hp > 0 && !(atk.Move.HasProbabilitiedAdditonalEffects() && (d.Ability.ShieldDust() || atk.Attacker.Ability.SheerForce())))
      {
        switch (Move.Class)
        {
          case MoveInnerClass.AttackWithTargetLv7DChange:
            d.ChangeLv7D(atk);
            break;
          case MoveInnerClass.AttackWithState:
            if (!Moves.CheckTriAttack(def) && atk.RandomHappen(Move.Attachment.Probability)) d.AddState(def);
            break;
        }
        if (!def.Ability.InnerFocus() && (atk.RandomHappen(Move.FlinchProbability, true) || Abilities.Stench(def) || Items.CanAttackFlinch(def))) d.OnboardPokemon.SetTurnCondition("Flinch");
        Abilities.PoisonTouch(def);
      }
      d.Ability.Attacked(def);//此时破格不能无视
      d.Item.Attacked(def);
      if (d.OnboardPokemon.HasCondition("Rage")) d.ChangeLv7D(d, StatType.Atk, 1, false, "Rage");
      atk.Attacker.CheckFaint();
      if (d.CheckFaint()) Triggers.KOed(def);
      else if (Move.MaxTimes > 1) d.Item.HpChanged(d);
    }
    protected virtual void PostEffect(AtkContext atk)
    {
      foreach (DefContext d in atk.Targets) Abilities.ColorChange(d);
      //红牌、逃生按钮
      Items.AttackPostEffect(atk);
    }
    protected override void MoveEnding(AtkContext atk)
    {
      base.MoveEnding(atk);
      if (atk.Move.MultiTurnAttack())
      {
        atk.Attachment--;
        if (atk.Attachment != 0) atk.Attacker.Action = PokemonAction.Moving;
        else if (atk.Move.MultiTurnAttackWithConfusion()) atk.Attacker.AddState(atk.Attacker, AttachedState.Confuse, false, 0, "EnConfuse2");
      }
      //3.因为逃生按钮下场的精灵选择换人
      if (atk.EjectButton != null) ;
    }
  }
}