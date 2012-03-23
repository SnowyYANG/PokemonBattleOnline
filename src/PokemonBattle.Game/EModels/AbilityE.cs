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

    void Raise();
    void Debut();
    void Attacked();
    void Lv8DChanging(ref StatType stat, ref int value);//性情乖僻
    void Lv8DChanged();
    void StateChanged();
    void KO();
    void CalculatingMoveType(ref BattleType type); //普通皮肤
    bool CanImplement(DefContext def); //auto raise
  }
}
