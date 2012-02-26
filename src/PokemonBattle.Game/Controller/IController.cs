using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Game
{
  /// <summary>
  /// 本接口是给技能、道具与特性特效脚本使用的
  /// </summary>
  public interface IController
  {
    Board Board { get; }
    List<PokemonProxy> ActivePokemons { get; } //押后等技能

    #region Service
    int GetRandomInt(int min, int max);
    #endregion

    #region Switch
    void Withdraw(int pmId);
    void Sendout(int pmId);
    void Sendout(Player player, int pmIndex);
    #endregion
  }
}
