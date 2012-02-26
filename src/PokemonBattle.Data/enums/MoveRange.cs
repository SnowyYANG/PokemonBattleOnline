using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Data
{
    public enum MoveRange
    {
        Invalid,
        Varies, //0D
        Field, //全场0A
        All, //所有精灵08
        Single,//单体选择00
        UserField,//本方场地0C
        UserOrParner, //本方随机01
        UserParty,//自己队伍与场上队友06 
        Partner,//本方选择02
        EnemyField, //对方场地0B
        RandomEnemy, //对方随机09
        AdjacentEnemies, //对方临近05
        User, //自己07
        Adjacent, //所有临近 自爆、冲浪04
        SingleEnemy, //先取03
    }
}
