using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.GameEvents;

namespace LightStudio.PokemonBattle.Game.Host
{
  public class AbilityE
  {
    private readonly int id;
    protected readonly Ability Ability;

    public AbilityE(int id)
    {
      this.id = id;
      Ability = DataService.GetAbility(id);
    }

    public int Id
    { get { return id; } }

    protected void Raise(PokemonProxy pm)
    {
      pm.Controller.ReportBuilder.Add(new AbilityEvent(pm));
    }

    public virtual bool CanAddState(PokemonProxy pm, PokemonProxy by, AttachedState state, bool showFail) { return true; }
    public virtual bool CanImplement(DefContext def) { return true; } //auto raise
    public virtual int Lv7DChanging(PokemonProxy pm, PokemonProxy by, StatType stat, int change, bool showFail) { return change; }
    public virtual Modifier ADSModifier(PokemonProxy pm, StatType stat) { return 0x1000; }
    public virtual Modifier PowerModifier(DefContext target) { return 0x1000; }
    public virtual Modifier AccuracyModifier(DefContext def) { return 0x1000; }

    public virtual void Attach(PokemonProxy pm) { }
    public virtual void Detach(PokemonProxy pm) { }
    public virtual void Attacked(DefContext def) { }
  }
}
