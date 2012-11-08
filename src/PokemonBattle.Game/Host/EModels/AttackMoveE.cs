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
      else if (!ability.RockHead()) aer.EffectHurt(-v, "ReHurt");
    }
    
    public AttackMoveE(int moveId)
      : base(moveId)
    {
    }

    public override void Execute(AtkContext atk)
    {
      if (PrepareOneTurn(atk.Attacker) && !Sp.Items.PowerHerb(atk.Attacker)) return;
      base.Execute(atk);
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
          def.Defender.Controller.ReportBuilder.Add("OHKO");
          def.Damage = def.Defender.Hp;
          MoveHurt e = new MoveHurt();
          def.Defender.Controller.ReportBuilder.Add(e);
          def.Defender.MoveHurt(def);
          e.SetHurt(defs);
          ImplementEffect(def);
          PassiveEffect(def);
        }
      }
      else
      {
        AtkContext atk = def.AtkContext;
        PokemonProxy a = atk.Attacker;
        bool allSub = true;
        if (!Move.Flags.IgnoreSubstitute)
          foreach (DefContext d in defs) allSub &= Sp.Conditions.Substitute.Hurt(d);
        if (!allSub)
        {
          foreach (DefContext d in defs)
            if (d.RemoveCondition("Antiberry")) d.Defender.RaiseItem();
          MoveHurt e = new MoveHurt();
          a.Controller.ReportBuilder.Add(e);
          foreach (DefContext d in defs)
          {
            d.Defender.MoveHurt(d);
            atk.TotalDamage += d.Damage;
          }
          e.SetHurt(defs);
        }

        if (Move.HurtPercentage > 0) DamagePercentage(def, Move.HurtPercentage);
        if (Move.Class == MoveInnerClass.AttackWithSelfLv7DChange && atk.RandomHappen(Move.Lv7DChanges.First().Probability)) a.ChangeLv7D(atk.Attacker, Move);

        foreach (DefContext d in defs)
          if (!d.HitSubstitute)
          {
            ImplementEffect(d);
            PassiveEffect(d);
          }

        if (a.Hp > 0)
        {
          if (Move.HurtPercentage < 0) DamagePercentage(def, Move.HurtPercentage);
          else if (Move.MaxHpPercentage < 0) //拼命专用
          {
            var change = a.Pokemon.Hp.Origin * Move.MaxHpPercentage / 100;
            a.Pokemon.SetHp(a.Hp + change == 0 ? -1 : change);
            a.OnboardPokemon.SetTurnCondition("Assurance");
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
        var aer = atk.Attacker;
        Items.CheckGem(atk);
        atk.CTLv = Move.CtLv;
        if (atk.CTLv < 5)
        {
          if (aer.OnboardPokemon.HasCondition("FocusEnergy")) atk.CTLv += 2;
          if (aer.Ability.SuperLuck()) atk.CTLv++;
          atk.CTLv += aer.Item.CtLvRevise(aer);
          if (atk.CTLv > 4) atk.CTLv = 4;
        }
        foreach (DefContext d in atk.Targets) CalculateDamage(d);
      }
    }
    protected virtual void CalculateDamage(DefContext def)
    {
      AtkContext atk = def.AtkContext;
      PokemonProxy aer = atk.Attacker;
      Controller c = aer.Controller;

      if (!(def.Defender.Tile.Field.HasCondition("LuckyChant") || def.Ability.CannotBeCted()))
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
          if (Move.UseDefenderAtk()) p = def.Defender.OnboardPokemon;
          else p = atk.Attacker.OnboardPokemon;
          a = p.FiveD.GetStat(st);
          if (!def.Ability.Unaware())
          {
            int atkLv = p.Lv5D.GetStat(st);
            if (!(def.IsCt && atkLv < 0)) a = OnboardPokemon.Get5D(a, atkLv);
          }
        }
        a *= Abilities.Hustle(atk);
        Modifier m = Abilities.ThickFat(def);
        m *= aer.Ability.AModifier(atk);
        m *= Abilities.FlowerGift(atk);
        m *= aer.Item.AModifier(def);
        def.Damage *= a * m;
      }
      {
        StatType st = Move.Category == MoveCategory.Physical ? StatType.Def : StatType.SpDef;
        int defRaw = def.Defender.OnboardPokemon.FiveD.GetStat(st);
        int defLv = 0;
        if (!(aer.Ability.Unaware() || Move.IgnoreDefenderLv7D())) defLv = def.Defender.OnboardPokemon.Lv5D.GetStat(st);
        if (def.IsCt && defLv > 0) defLv = 0;
        int d = OnboardPokemon.Get5D(defRaw, defLv);
        Modifier m = aer.Ability.IgnoreDefenderAbility() ? 0x1000 : Abilities.DModifier(def);
        m *= def.Defender.Item.DModifier(def);
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
      if (Move.Category == MoveCategory.Physical && aer.State == PokemonState.BRN && !aer.Ability.Guts()) def.Damage >>= 1;
      //8.Make sure damage is at least 1
      if (def.Damage < 1) def.Damage = 1;
      //9.Apply the final modifier
      def.Damage *= Triggers.DamageFinalModifier(def, DamageFinalModifier(def));
    }
    protected internal virtual Modifier DamageFinalModifier(DefContext def)
    { return 0x1000; }
    protected virtual void ImplementEffect(DefContext def)
    {
      PokemonProxy d = def.Defender;
      if (d.Hp > 0)
      {
        if (Move.Class == MoveInnerClass.AttackWithTargetLv7DChange) d.ChangeLv7D(def);
        else if (Move.Class == MoveInnerClass.AttackWithState) d.AddState(def);
        if (!def.Ability.InnerFocus() && (Move.FlinchProbability != 0 && def.RandomHappen(Move.FlinchProbability) || Abilities.Stench(def) || Items.CanAttackFlinch(def))) d.OnboardPokemon.SetTurnCondition("Flinch");
      }
    }
    protected virtual void PassiveEffect(DefContext def)
    {
      PokemonProxy d = def.Defender;
      Abilities.PoisonTouch(def);
      d.Ability.Attacked(def);//此时破格不能无视
      d.Item.Attacked(def);
      if (d.OnboardPokemon.HasCondition("Rage")) d.ChangeLv7D(d, StatType.Atk, 1, false, "Rage");
      def.AtkContext.Attacker.CheckFaint();
      if (d.CheckFaint()) Triggers.KOed(def, d.OnboardPokemon);
      else if (Move.MaxTimes > 1) d.Item.HpChanged(d);
    }
    protected virtual void PostEffect(AtkContext atk)
    {
      foreach (DefContext d in atk.Targets) Abilities.ColorChange(d);
      Items.AttackPostEffect(atk);
    }
    protected override void MoveEnding(AtkContext atk)
    {
      base.MoveEnding(atk);
      {
        var o = atk.GetCondition("MultiTurn");
        if (o != null)
        {
          o.Turn--;
          if (o.Turn != 0) atk.Attacker.Action = PokemonAction.Moving;
          else if (o.Bool) atk.Attacker.AddState(atk.Attacker, AttachedState.Confuse, false, 0, "EnConfuse2");
        }
      }
      {
        var o = atk.GetCondition<Tile>("EjectButton");
        if (o != null) atk.Controller.PauseForSendoutInput(o);
      }
    }
  }
}