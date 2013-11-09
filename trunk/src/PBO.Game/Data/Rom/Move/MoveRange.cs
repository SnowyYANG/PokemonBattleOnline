using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Game
{
  public enum MoveRange
  {
    /// <summary>
    /// 单体选择00
    /// </summary>
    Single,
    /// <summary>
    /// 本方随机01
    /// </summary>
    RandomSelfPokemon,
    /// <summary>
    /// 本方选择02
    /// </summary>
    SingleAlly,
    /// <summary>
    /// 对方选择03
    /// </summary>
    SingleFoe,
    /// <summary>
    /// 所有临近 自爆、冲浪04
    /// </summary>
    Adjacent,
    /// <summary>
    /// 对方临近05
    /// </summary>
    FoePokemons,
    /// <summary>
    /// 自己队伍与场上队友06
    /// </summary>
    SelfPokemons,
    /// <summary>
    /// 自己07
    /// </summary>
    Self,
    /// <summary>
    /// 所有精灵08
    /// </summary>
    All,
    /// <summary>
    /// 对方随机09
    /// </summary>
    RandomFoePokemon,
    /// <summary>
    /// 全场0A
    /// </summary>
    Board,
    /// <summary>
    /// 对方场地0B
    /// </summary>
    FoeField,
    /// <summary>
    /// 本方场地0C
    /// </summary>
    SelfField,
    /// <summary>
    /// 0D
    /// </summary>
    Varies,
  }
}
