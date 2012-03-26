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
    bool IgnoreDefenderAbility { get; }

    bool CanWithdraw(PokemonProxy pm);
    bool CanChangeState(PokemonState state);
    bool CanImplement(DefContext def); //auto raise
    double Get5DRevise(PokemonProxy pm, StatType stat);

    void Debut(PokemonProxy pm);
    void Attacked(DefContext def);
    void Lv8DChanging(ref StatType stat, ref int value);//性情乖僻
    void Lv8DChanged();
    void StateChanged(PokemonProxy sub, PokemonProxy obj, PokemonState state);//同步率
    void KO(DefContext atk); //自信过剩，不是引爆
    void CalculatingMoveType(ref BattleType type); //普通皮肤
    void CalculatingAccuracy(AtkContext atk);
    void CalculatingAccuracy(DefContext def);
  }

  internal class AbilityE0 : IAbilityE
  {
    public int Id { get { return 0; } }
    public bool IgnoreDefenderAbility { get { return false; } }

    public bool CanWithdraw(PokemonProxy pm) { return true; }
    public bool CanChangeState(PokemonState state) { return true; }
    public bool CanImplement(DefContext def) { return true; } //auto raise
    public double Get5DRevise(PokemonProxy pm, StatType stat) { return 1; }

    public void Debut(PokemonProxy pm) { }
    public void Attacked(DefContext def) { }
    public void Lv8DChanging(ref StatType stat, ref int value) { }//性情乖僻
    public void Lv8DChanged() { }
    public void StateChanged(PokemonProxy sub, PokemonProxy obj, PokemonState state) { }//同步率
    public void KO(DefContext atk) { } //自信过剩，不是引爆
    public void CalculatingMoveType(ref BattleType type) { } //普通皮肤
    public void CalculatingAccuracy(AtkContext atk) { }
    public void CalculatingAccuracy(DefContext def) { }
  }
}