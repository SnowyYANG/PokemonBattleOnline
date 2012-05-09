using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Interactive.GameEvents;

namespace LightStudio.PokemonBattle.Game
{
  public class AbilityE : IAbilityE
  {
    private readonly int id;
    protected readonly Ability Ability;

    public AbilityE(int id, string logKey = null)
    {
      this.id = id;
      Ability = DataService.GetAbility(id);
    }
    int IAbilityE.Id
    { get { return id; } }

    protected void Raise(PokemonProxy pm)
    {
      pm.Controller.ReportBuilder.Add(new AbilityEvent(pm));
    }

    public virtual bool CanWithdraw(PokemonProxy pm) { return true; }
    public virtual bool CanAddState(PokemonProxy by, AttachedState state) { return true; }
    public virtual bool CanImplement(DefContext def) { return true; } //auto raise
    public virtual double Get5DRevise(PokemonProxy pm, StatType stat) { return 1; }

    public virtual void Attach(PokemonProxy pm) { }
    public virtual void Attacked(DefContext def) { }
    public virtual void Lv7DChanging(ref StatType stat, ref int value) { }
    public virtual void CalculatingAccuracy(AtkContext atk) { }
    public virtual void CalculatingAccuracy(DefContext def) { }
    public virtual void CalculatingPower(AtkContext atk) { }
  }
}
