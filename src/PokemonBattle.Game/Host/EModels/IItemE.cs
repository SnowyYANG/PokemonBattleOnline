using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  public interface IItemE
  {
    int Id { get; }

    bool CanLost(PokemonProxy pm);
    int CompareValue(PokemonProxy pm);
    double Get5DRevise(PokemonProxy pm, StatType stat);
    int GetCtLvRevise(PokemonProxy pm);

    void Raise(PokemonProxy pm);

    void Attach(PokemonProxy pm); //树果
    void HpChanged(PokemonProxy pm);
    /// <summary>
    /// 红线、柿果、精神香草、5树果
    /// </summary>
    void StateAdded(PokemonProxy pm, AttachedState state);
    void CalculatingAccuracy(AtkContext atk);
    void CalculatingAccuracy(DefContext def);
    void CalculatingPower(AtkContext atk);
    void ReviseDamage2(AtkContext atk);
    void ReviseDamage3(DefContext def);
    void Attacked(DefContext def);
  }

  public static partial class GameService
  {
    private sealed class ItemE0 : IItemE
    {
      public int Id { get { return 0; } }

      public bool CanLost(PokemonProxy pm) { return true; }
      public int CompareValue(PokemonProxy pm) { return 0; }
      public double Get5DRevise(PokemonProxy pm, StatType stat) { return 1; }
      public int GetCtLvRevise(PokemonProxy pm) { return 0; }

      public void Raise(PokemonProxy pm) { }

      public void Attach(PokemonProxy pm) { }
      public void HpChanged(PokemonProxy pm) { }
      public void ReviseDamage2(AtkContext atk) { }
      public void StateAdded(PokemonProxy pm, AttachedState state) { }
      public void CalculatingAccuracy(AtkContext atk) { }
      public void CalculatingAccuracy(DefContext def) { }
      public void CalculatingPower(AtkContext atk) { }
      public void ReviseDamage3(DefContext def) { }
      public void Attacked(DefContext def) { }
    }
  }
}