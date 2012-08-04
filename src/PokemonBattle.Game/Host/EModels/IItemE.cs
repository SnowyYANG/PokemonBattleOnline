using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Host
{
  public interface IItemE
  {
    int Id { get; }
    string LogKey { get; }

    bool CanLost(PokemonProxy pm);
    int CompareValue(PokemonProxy pm);
    /// <summary>
    /// Atk/Def/SpAtk/SpDef/Speed
    /// </summary>
    Modifier ADSModifier(PokemonProxy pm, StatType stat);
    Modifier DamageFinalModifier(DefContext def);
    Modifier PowerModifier(AtkContext atk);
    int GetCtLvRevise(PokemonProxy pm);

    void Raise(PokemonProxy pm, string key = null);

    void Attach(PokemonProxy pm); //树果
    void HpChanged(PokemonProxy pm);
    /// <summary>
    /// 红线、柿果、精神香草、5树果
    /// </summary>
    void StateAdded(PokemonProxy pm, AttachedState state);
    void Attacked(DefContext def);

  }

  public static partial class EffectsService
  {
    private sealed class ItemE0 : IItemE
    {
      public int Id { get { return 0; } }
      public string LogKey { get { return null; } }

      public bool CanLost(PokemonProxy pm) { return true; }
      public int CompareValue(PokemonProxy pm) { return 0; }
      public Modifier ADSModifier(PokemonProxy pm, StatType stat) { return 0x1000; }
      public Modifier DamageFinalModifier(DefContext def) { return 0x1000; }
      public Modifier PowerModifier(AtkContext atk) { return 0x1000; }
      public int GetCtLvRevise(PokemonProxy pm) { return 0; }

      public void Raise(PokemonProxy pm, string key) { }

      public void Attach(PokemonProxy pm) { }
      public void HpChanged(PokemonProxy pm) { }
      public void StateAdded(PokemonProxy pm, AttachedState state) { }
      public void Attacked(DefContext def) { }
    }
  }
}