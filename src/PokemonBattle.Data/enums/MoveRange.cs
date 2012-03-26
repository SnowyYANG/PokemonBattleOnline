using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Data
{
  public enum MoveRange
  {
    Invalid,
    /// <summary>
    /// 0D
    /// </summary>
    Varies,
    /// <summary>
    /// 全场0A
    /// </summary>
    Field,
    /// <summary>
    /// 所有精灵08
    /// </summary>
    All,
    /// <summary>
    /// 单体选择00
    /// </summary>
    Single,
    /// <summary>
    /// 本方场地0C
    /// </summary>
    UserField,
    /// <summary>
    /// 本方随机01
    /// </summary>
    UserOrParner,
    /// <summary>
    /// 自己队伍与场上队友06
    /// </summary>
    UserParty,
    /// <summary>
    /// 本方选择02
    /// </summary>
    Partner,
    /// <summary>
    /// 对方场地0B
    /// </summary>
    EnemyField,
    /// <summary>
    /// 对方随机09
    /// </summary>
    RandomEnemy,
    /// <summary>
    /// 对方临近05
    /// </summary>
    AdjacentEnemies,
    /// <summary>
    /// 自己07
    /// </summary>
    User,
    /// <summary>
    /// 所有临近 自爆、冲浪04
    /// </summary>
    Adjacent,
    /// <summary>
    /// 先取03
    /// </summary>
    SingleEnemy,
  }
}
