using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Game
{
    public interface ITeamIO
    {
        /// <summary>
        /// 读取队伍
        /// </summary>
        PokemonBT Read(string path);

        /// <summary>
        /// 输出队伍
        /// </summary>
        void Write(PokemonBT pds, string path);

        /// <summary>
        /// 输出字符串
        /// </summary>
        string ExportString(PokemonBT pds);

        /// <summary>
        /// 加载字符串
        /// </summary>
        PokemonBT ImportString(string body);
    }
}
