using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.PBO.Editor
{
  internal static class EditorConfig
  {
    internal const string LAST_POKEMON_TYPE = "LastUsedPokemonType";
    private static bool isInited;

    internal static void Init()
    {
      if (isInited) return;
      UIElements.Config.Register(LAST_POKEMON_TYPE, 1);
      isInited = true;
    }
  }
}
