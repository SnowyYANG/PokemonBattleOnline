using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game
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
      count--;//gamelog
      System.Diagnostics.Debugger.Break();
    }
  }
  public abstract class PmCondition : Condition
  {
    protected readonly PokemonProxy pm;

    protected PmCondition(string name, PokemonProxy pm, int count = 0)
      : base(name, pm == null ? null : pm.OnboardPokemon, count)
    {
      this.pm = pm;
    }

    protected void AddReport(Interactive.GameEvent e)
    {
      pm.Controller.ReportBuilder.Add(e);
    }
    protected void AddReportPm(string key)
    {
      pm.AddReportPm(key);
    }
    public virtual bool CanExecute()
    {
      return true;
    }
  }
}
