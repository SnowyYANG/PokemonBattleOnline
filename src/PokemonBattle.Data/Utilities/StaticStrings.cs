using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.Data
{
    internal static class CONSTS
    {
        public const string STRING_RESOURCE_FORMAT = @"string\{0}.xml";
        public const string DATA_STRING_RESOURCE_FORMAT = @"string\data\{0}.xml";
        public const string LOG_RESOURCE_FORMAT = @"log\{0}.xml";
        
        public const string INDEX_FILE = "index.xml";
        public const string CONFIG_FILE = "config.xml";

        public const string BATTLE_DOMAIN = "PokemonBattle";
        public const string BATTLE_DATA_DOMAIN = "PokemonBattleData";
        public const string GAME_LOG_DOMAIN = "PokemonBattleGameLog";

        public const string USER_DATA_FILE = "user_pm.dat";
    }
}
