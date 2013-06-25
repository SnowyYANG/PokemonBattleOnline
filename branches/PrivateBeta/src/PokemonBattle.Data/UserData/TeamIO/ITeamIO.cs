using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Data
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
        void Write(PokemonBT bt, string path);

        /// <summary>
        /// 输出字符串
        /// </summary>
        string ExportString(PokemonBT bt);

        /// <summary>
        /// 加载字符串
        /// </summary>
        PokemonBT ImportString(string body);
    }
}