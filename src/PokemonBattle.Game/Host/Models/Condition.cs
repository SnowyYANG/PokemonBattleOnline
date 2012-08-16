using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game.Host
{
  public abstract class Condition
  {
    public readonly string Name;
    protected readonly ConditionalObject obj;
    protected int count;

    protected Condition(string name, ConditionalObject obj, int count = 0)
    {
      Name = name;
    }

    public void Remove()
    {
      if (obj != null) obj.RemoveCondition(Name);
    }
    public virtual void CountDown()
    {
      count--;
    }
  }
  public abstract class PmCondition : Condition
  {
    protected readonly PokemonProxy Pm;

    protected PmCondition(string name, PokemonProxy pm, int count = 0)
      : base(name, pm == null ? null : pm.OnboardPokemon, count)
    {
      this.Pm = pm;
    }

    protected void AddReport(GameEvent e)
    {
      Pm.Controller.ReportBuilder.Add(e);
    }
    protected void AddReportPm(string key)
    {
      Pm.AddReportPm(key);
    }
    protected void AddResetYReport(string key, object arg1 = null, object arg2 = null)
    {
      AddReport(GameEvents.PositionChange.Reset(key, Pm, arg1, arg2));
    }
    public virtual bool CanExecute()
    {
      return true;
    }
  }
}
