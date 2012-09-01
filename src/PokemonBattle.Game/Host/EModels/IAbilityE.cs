using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Host
{
  public interface IAbilityE
  {
    int Id { get; }

    bool CanAddState(PokemonProxy pm, PokemonProxy by, AttachedState state, bool showFail);
    bool CanImplement(DefContext def);
    int Lv7DChanging(PokemonProxy pm, PokemonProxy by, StatType stat, int change, bool showFail);//性情乖僻单纯白烟净体
    /// <summary>
    /// Atk/Def/SpAtk/SpDef/Speed
    /// </summary>
    Modifier ADSModifier(PokemonProxy pm, StatType stat);
    Modifier PowerModifier(DefContext target);
    Modifier AccuracyModifier(DefContext def);

    void Attach(PokemonProxy pm);
    void Detach(PokemonProxy pm);
    void Attacked(DefContext def);
  }

  public static partial class EffectsService
  {
    private sealed class AbilityE0 : IAbilityE
    {
      int IAbilityE.Id { get { return 0; } }

      bool IAbilityE.CanAddState(PokemonProxy pm, PokemonProxy by, AttachedState state, bool showFail) { return true; }
      bool IAbilityE.CanImplement(DefContext def) { return true; }
      int IAbilityE.Lv7DChanging(PokemonProxy pm, PokemonProxy by, StatType stat, int change, bool showFail) { return change; }
      Modifier IAbilityE.ADSModifier(PokemonProxy pm, StatType stat) { return 0x1000; }
      Modifier IAbilityE.PowerModifier(DefContext target) { return 0x1000; }
      Modifier IAbilityE.AccuracyModifier(DefContext def) { return 0x1000; }
      void IAbilityE.Attach(PokemonProxy pm) { }
      void IAbilityE.Detach(PokemonProxy pm) { }
      void IAbilityE.Attacked(DefContext def) { }
    }
  }
}