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
    double Get5DRevise(PokemonProxy pm, StatType stat);

    void Attach(PokemonProxy pm);
    void Attacked(DefContext def);
    void Lv7DChanging(ref StatType stat, ref int value);//性情乖僻
    void CalculatingAccuracy(AtkContext atk);
    void CalculatingAccuracy(DefContext def);
    void CalculatingPower(AtkContext atk);
  }

  public static partial class GameService
  {
    private sealed class AbilityE0 : IAbilityE
    {
      public int Id { get { return 0; } }

      public bool CanWithdraw(PokemonProxy pm) { return true; }
      public bool CanAddState(PokemonProxy by, AttachedState state) { return true; }
      public bool CanImplement(DefContext def) { return true; } //auto raise
      public double Get5DRevise(PokemonProxy pm, StatType stat) { return 1; }

      public void Attach(PokemonProxy pm) { }
      public void Attacked(DefContext def) { }
      public void Lv7DChanging(ref StatType stat, ref int value) { }//性情乖僻
      public void CalculatingAccuracy(AtkContext atk) { }
      public void CalculatingAccuracy(DefContext def) { }
      public void CalculatingPower(AtkContext atk) { }
    }
  }
}