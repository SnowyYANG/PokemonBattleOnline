using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game.Host.Triggers;

namespace PokemonBattleOnline.Game.Host
{
  internal static partial class PTs
  {
    public static void ShowLogPm(this PokemonProxy pm, string key, int arg1 = 0, int arg2 = 0)
    {
      pm.Controller.ReportBuilder.ShowLog(key, pm.Id, arg1, arg2);
    }
    public static void NoEffect(this PokemonProxy pm)
    {
      ShowLogPm(pm, "NoEffect");
    }
    public static bool CanHpRecover(this PokemonProxy pm, bool showFail = false)
    {
      if (pm.AliveOnboard)
      {
        if (pm.Hp == pm.Pokemon.MaxHp)
        {
          if (showFail) ShowLogPm(pm, "FullHp");
          return false;
        }
        if (pm.OnboardPokemon.HasCondition("HealBlock"))
        {
          ShowLogPm(pm, "HealBlock");
          return false;
        }
        return true;
      }
      return false;
    }
    public static bool CanChangeForm(this PokemonProxy pm, int number)
    {
      return pm.Pokemon.Form.Species.Number == number && !pm.OnboardPokemon.HasCondition("Transform");
    }
    public static bool CanChangeForm(this PokemonProxy pm, int number, int form)
    {
      return pm.OnboardPokemon.Form.Index != form && CanChangeForm(pm, number);
    }
    public static void ChangeForm(this PokemonProxy pm, int form, bool forever = false, string log = "FormChange")
    {
      pm.OnboardPokemon.ChangeForm(pm.OnboardPokemon.Form.Species.GetForm(form));
      if (forever) pm.Pokemon.Form = pm.OnboardPokemon.Form;
      pm.Controller.ReportBuilder.ChangeForm(pm);
      if (log != null) ShowLogPm(pm, log);
      AbilityAttach.Execute(pm);
    }
    /// <summary>
    /// null log to show default log
    /// </summary>
    /// <param name="log"></param>
    /// <param name="arg1"></param>
    public static void DeAbnormalState(this PokemonProxy pm, string log = null, int arg1 = 0)
    {
      if (pm.State != PokemonState.Normal && pm.Hp > 0)
      {
        ShowLogPm(pm, log ?? "De" + pm.State, arg1);
        pm.Pokemon.State = PokemonState.Normal;
      }
    }
    public static int CanChangeLv7D(this PokemonProxy pm, PokemonProxy by, StatType stat, int change, bool showFail)
    {
      if (!pm.AliveOnboard || change == 0) return 0;
      change = Lv7DChanging.Execute(pm, by, stat, change, showFail);
      if (change != 0)
      {
        int oldValue = stat == StatType.Accuracy ? pm.OnboardPokemon.AccuracyLv : stat == StatType.Evasion ? pm.OnboardPokemon.EvasionLv : pm.OnboardPokemon.Lv5D.GetStat(stat);
        if (oldValue == 6 && change > 0)
        {
          if (showFail) ShowLogPm(pm, "7DMax", (int)stat);
          return 0;
        }
        else if (oldValue == -6 && change < 0)
        {
          if (showFail) ShowLogPm(pm, "7DMin", (int)stat);
          return 0;
        }
        int value = oldValue + change;
        if (value > 6) change = 6 - oldValue;
        else if (value < -6) change = -6 - oldValue;
      }
      return change;
    }
    public static bool CheckFaint(this PokemonProxy pm)
    {
      if (pm.Hp == 0 && pm.OnboardPokemon != pm.NullOnboardPokemon)
      {
        pm.Field.SetCondition("FaintTurn", pm.Controller.TurnNumber);
        pm.Pokemon.State = PokemonState.Faint;
        pm.Controller.Withdraw(pm, "Faint", 0, false);
        return true;
      }
      return false;
    }
    public static void ConsumeItem(this PokemonProxy pm, bool cheekPouch = true)
    {
      pm.OnboardPokemon.SetTurnCondition("UsedItem", pm.Pokemon.Item);
      pm.Field.SetCondition("UsedItem" + pm.Id, pm.Pokemon.Item);
      if (ITs.Berry(pm.Pokemon.Item))
      {
        pm.OnboardPokemon.SetCondition("Belch");
        pm.Field.SetCondition("UsedBerry" + pm.Id, pm.Pokemon.Item);
        if (CanHpRecover(pm) && ATs.RaiseAbility(pm, As.CHEEK_POUCH)) HpRecoverByOneNth(pm, 3);
      }
      RemoveItem(pm);
    }
    /// <summary>
    /// Item should not be null, or Unburden effect will be wrong
    /// </summary>
    public static void RemoveItem(this PokemonProxy pm)
    {
      pm.Pokemon.Item = 0;
      if (pm.AbilityE(As.UNBURDEN)) pm.OnboardPokemon.SetCondition("Unburden");
    }
    public static void SetItem(this PokemonProxy pm, int item)
    {
      pm.Pokemon.Item = item;
      pm.OnboardPokemon.RemoveCondition("Unburden");
      pm.OnboardPokemon.RemoveCondition("ChoiceItem");
    }
    public static void ChangeAbility(this PokemonProxy pm, int ab)
    {
      AbilityDetach.Execute(pm);
      pm.OnboardPokemon.Ability = ab;
      AbilityAttach.Execute(pm);
    }

    public static void CalculatePriority(this PokemonProxy pm)
    {
      pm.Priority = 0;
      pm.ItemSpeedValue = 0;
      if (pm.Action != PokemonAction.WillSwitch)
      {
        var m = pm.SelectedMove.Type;
        pm.Priority = m.Priority;
        if (m.Category == MoveCategory.Status && pm.AbilityE(As.PRANKSTER) || m.Type == BattleType.Flying && pm.AbilityE(As.GALE_WINGS)) pm.Priority++;
        switch (pm.Item)
        {
          case Is.LAGGING_TAIL:
          case Is.FULL_INCENSE:
            pm.ItemSpeedValue = -1;
            break;
          case Is.QUICK_CLAW:
            if (pm.Controller.RandomHappen(20))
            {
              ShowLogPm(pm, "QuickItem", Is.QUICK_CLAW);
              pm.ItemSpeedValue = 1;
            }
            break;
          case Is.CUSTAP_BERRY:
            if (ATs.Gluttony(pm))
            {
              ShowLogPm(pm, "QuickItem", Is.CUSTAP_BERRY);
              pm.ConsumeItem();
              pm.ItemSpeedValue = 1;
            }
            break;
        }
      }
    }
  }
}
