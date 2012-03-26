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
    double Get5DRevise(PokemonProxy pm, StatType stat);

    void Raise(PokemonProxy pm);
    void Debut(PokemonProxy pm);
    void HpChanged(PokemonProxy pm);
    void ImplementMove(AtkContext atk);
    void ConditionAdded(PokemonProxy pm, string condition); //红线
    void CalculatingAccuracy(AtkContext atk);
    void CalculatingAccuracy(DefContext def);
  }

  internal class ItemE0 : IItemE
  {
    public int Id { get { return 0; } }

    public bool CanLost(PokemonProxy pm) { return true; }
    public double Get5DRevise(PokemonProxy pm, StatType stat) { return 1; }

    public void Raise(PokemonProxy pm) { }
    public void Debut(PokemonProxy pm) { }
    public void HpChanged(PokemonProxy pm) { }
    public void ImplementMove(AtkContext atk) { }
    public void ConditionAdded(PokemonProxy pm, string condition) { }
    public void CalculatingAccuracy(AtkContext atk) { }
    public void CalculatingAccuracy(DefContext def) { }
  }
}