using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  public interface IAbilityE
  {
    int Id { get; }

    bool CanWithdraw(PokemonProxy pm);
    bool CanAddState(PokemonProxy by, AttachedState state);
    bool CanImplement(DefContext def); //auto raise
    /// <summary>
    /// Atk/Def/SpAtk/SpDef/Speed
    /// </summary>
    Modifier ADSModifier(PokemonProxy pm, StatType stat);
    Modifier PowerModifier(DefContext target);
    Modifier AccuracyModifier(DefContext def);

    void Attach(PokemonProxy pm);
    void Attacked(DefContext def);
    void Lv7DChanging(ref StatType stat, ref int value);//性情乖僻

  }

  public static partial class GameService
  {
    private sealed class AbilityE0 : IAbilityE
    {
      int IAbilityE.Id { get { return 0; } }

      bool IAbilityE.CanWithdraw(PokemonProxy pm) { return true; }
      bool IAbilityE.CanAddState(PokemonProxy by, AttachedState state) { return true; }
      bool IAbilityE.CanImplement(DefContext def) { return true; } //auto raise
      Modifier IAbilityE.ADSModifier(PokemonProxy pm, StatType stat) { return 0x1000; }
      Modifier IAbilityE.PowerModifier(DefContext target) { return 0x1000; }
      Modifier IAbilityE.AccuracyModifier(DefContext def) { return 0x1000; }
      void IAbilityE.Attach(PokemonProxy pm) { }
      void IAbilityE.Attacked(DefContext def) { }
      void IAbilityE.Lv7DChanging(ref StatType stat, ref int value) { }//性情乖僻
    }
  }
}