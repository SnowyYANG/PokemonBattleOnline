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
    protected readonly string LogKey;

    public AbilityE(int id, string logKey = null)
    {
      this.id = id;
      Ability = DataService.GetAbility(id);
      LogKey = logKey;
    }
    int IAbilityE.Id
    { get { return id; } }
    public bool IgnoreDefenderAbility { get { return false; } }

    protected void Raise(PokemonProxy pm)
    {
      pm.Controller.ReportBuilder.Add(new AbilityEvent(pm, Ability, LogKey));
    }

    public virtual bool CanWithdraw(PokemonProxy pm) { return true; }
    public virtual bool CanChangeState(PokemonState state) { return true; }
    public virtual bool CanImplement(DefContext def) { return true; } //auto raise
    public virtual double Get5DRevise(PokemonProxy pm, StatType stat) { return 1; }

    public virtual void Debut(PokemonProxy pm) { }
    public virtual void Attacked(DefContext def) { }
    public virtual void Lv8DChanging(ref StatType stat, ref int value) { }
    public virtual void Lv8DChanged() { }
    public virtual void StateChanged(PokemonProxy sub, PokemonProxy obj, PokemonState state) { }
    public virtual void KO(DefContext atk) { }
    public virtual void CalculatingMoveType(ref BattleType type) { }
    public virtual void CalculatingAccuracy(AtkContext atk) { }
    public virtual void CalculatingAccuracy(DefContext def) { }
  }
}
