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
    public AttackMoveE(MoveType move)
      : base(move)
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
          a.Controller.ReportBuilder.Add("Hits", a, m.ToString());

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
      if (Move.AdvancedFlags.PrepareOneTurn && PrepareOneTurn(pm))
        return;
      AtkContext atk = new AtkContext(pm, Move);
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
      throw new NotImplementedException();
    }
    protected virtual void CalculatePower(AtkContext atk)
    {
      atk.Power = Move.Power;
    }
    protected virtual void Calculate(AtkContext atk) //固定伤害、大爆炸、佯攻override
    {
      PokemonProxy pm = atk.Attacker;

      atk.CTLv = Move.CtLv;
      if (!Moves.CalculateFoulPlayAtk(atk))
      {
        StatType st = Move.Category == MoveCategory.Physical ? StatType.Atk : StatType.SpAtk;
        atk.AtkRaw = pm.OnboardPokemon.Static.GetStat(st);
        atk.AtkRaw = (int)(atk.AtkRaw * pm.Ability.Get5DRevise(pm, st));
        atk.AtkRaw = (int)(atk.AtkRaw * pm.Item.Get5DRevise(pm, st));
        atk.AtkLv = pm.OnboardPokemon.Lv5D.GetStat(st);
      }
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
      //计算技能实际威力
      CalculatePower(atk);
      atk.Attacker.Ability.CalculatingPower(atk);
      atk.Attacker.Item.CalculatingPower(atk);
      if (atk.Type == BattleType.Fire)
      {
        if (pm.Controller.Board.HasCondition("WaterSport")) atk.Power >>= 1;
      }
      else if (atk.Type == BattleType.Electric && pm.Controller.Board.HasCondition("MudSport"))
        atk.Power >>= 1;
      //计算防御方对威力的修正
      if (atk.Power > 1)
        foreach (DefContext def in atk.Targets)
          Abilities.CalculatePowerRevise(def);    
      
      //双打
      if (atk.MultiTargets) atk.DamageRevise1.Enqueue(0.75);
      //天气
      Weather w = atk.Controller.GetAvailableWeather();
      BattleType type = atk.Type;
      if (w == Weather.IntenseSunlight)
      {
        if (type == BattleType.Water) atk.DamageRevise1.Enqueue(0.5);
        else if (type == BattleType.Fire) atk.DamageRevise1.Enqueue(1.5);
      }
      else if (w == Weather.HeavyRain)
      {
        if (type == BattleType.Water) atk.DamageRevise1.Enqueue(1.5);
        else if (type == BattleType.Fire) atk.DamageRevise1.Enqueue(0.5);
      }
      Moves.CheckSolarbeam(atk);
      //引火
      Sp.Conditions.FlashFire.ReviseDamage1(atk);
      //宝石、节拍器、生命玉
      atk.Attacker.Item.ReviseDamage2(atk);
      //本属性修正
      if (atk.Attacker.OnboardPokemon.HasType(atk.Type))
        atk.DamageRevise3 = atk.Attacker.Ability.Adaptability() ? 2 : 1.5;
    }
    protected virtual void Calculate(DefContext def) //固定伤害override
    {
      AtkContext atk = def.AtkContext;
      PokemonProxy aer = atk.Attacker;
      Controller c = aer.Controller;
      StatType st = Move.Category == MoveCategory.Physical ? StatType.Def : StatType.SpDef;
      int defRaw = 0;
      int atkLv = 0;
      int defLv = 0;

      if (!(c.Board[def.Defender.Pokemon.TeamId].HasCondition("LuckyChant") || def.Ability.CannotBeCted()))
        if (Move.CtLv > 5) def.IsCt = true;
        else def.IsCt = c.OneNth(LV_CT[atk.CTLv]);

      defRaw = def.Defender.OnboardPokemon.Static.GetStat(st);
      if (!def.Ability.Unaware()) atkLv = atk.AtkLv;
      if (!aer.Ability.Unaware()) defLv = def.Defender.OnboardPokemon.Lv5D.GetStat(st);

      def.Damage = (int)(aer.Pokemon.Lv * 0.4 + 2);
      def.Damage *= (int)(atk.Power * def.PowerRevise);
      def.Damage *= OnboardPokemon.Get5D(atk.AtkRaw, atkLv);
      def.Damage /= OnboardPokemon.Get5D(defRaw, defLv);
      def.Damage /= 50;
      //修正1
      {
        //反射盾·光之壁
#warning
        //攻击方（双打、天气修正、引火）
        foreach(double r in atk.DamageRevise1)
          def.Damage = (int)(def.Damage * r);
      }
      def.Damage += 2;
      //修正2
      {
        //会心一击
        if (def.IsCt) def.Damage *= aer.Ability.Sniper()? 3 : 2;
        //先取
#warning
        //宝石、节拍器、生命玉
        def.Damage = (int)(def.Damage * atk.DamageRevise2);
      }
      def.Damage = (int)(def.Damage * (aer.Controller.GetRandomInt(85, 100) / 100d));
      //随机数后
      {
        //属性加成、适应力（属性相克前）
        def.Damage = (int)(def.Damage * atk.DamageRevise3);
        //属性相克
        def.Damage = (int)(def.Damage * def.EffectRevise);
        //有色眼镜（属性相克后）
        Abilities.CheckTintedLens(def);
        //达人腰带（属性相克后）
        Items.CheckExpertBelt(def);
        //过滤器、坚岩
        Abilities.CheckFilterSolidRock(def);
        //抗属性果（属性相克后）
        def.Defender.Item.ReviseDamage3(def);
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