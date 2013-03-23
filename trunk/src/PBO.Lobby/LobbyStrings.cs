using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.PBO.Lobby
{
  internal static class LS
  {
    public static string LOGINFAILED_NAME
    { get { return "不能使用的登陆名。"; } }
    public static string LOGINFAILED_VERSION
    { get { return "客户端与服务器的版本不相互兼容。"; } }
    public static string LOGINFAILED_FULL
    { get { return "服务器已满。"; } }
  }
}
