using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Data
{
    public interface ITeamIO
    {
        /// <summary>
        /// 读取队伍
        /// </summary>
        PokemonCollection Read(string path);

        /// <summary>
        /// 输出队伍
        /// </summary>
        void Write(IEnumerable<IPokemonData> pds, string path);

        /// <summary>
        /// 输出字符串
        /// </summary>
        string ExportString(IEnumerable<IPokemonData> pds);

        /// <summary>
        /// 加载字符串
        /// </summary>
        PokemonCollection ImportString(string body);
    }
}
