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
    event Action PokemonWithdrawing;
    event Action PokemonSendout;
    /// <summary>
    /// do NOT add or remove, sort and randomly access only
    /// </summary>
    List<IPokemonProxy> OnboardPokemons { get; } //押后等技能

    #region Service
    int GetRandomInt(int min, int max);
    #endregion

    #region Switch
    bool CanWithdraw(IPokemonProxy pm);
    bool CanSendout(Pokemon pm, Position position);
    bool Withdraw(IPokemonProxy pm);
    bool Sendout(Pokemon pm, Position position);
    #endregion
  }
}
