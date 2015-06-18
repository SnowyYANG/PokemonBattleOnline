using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Game
{
    public enum GameMode : byte
    {
        Single,
        Double,
        Tag,
        Triple,
        Rotation,
        Sky,
        Inverse
    }
    public static class GameModeExtensions
    {
        public static int XBound(this GameMode mode)
        {
            switch (mode)
            {
                case GameMode.Tag:
                case GameMode.Double:
                    return 2;
                case GameMode.Triple:
                case GameMode.Rotation:
                    return 3;
                default:
                    return 1;
            }
        }
        public static int PlayersPerTeam(this GameMode mode)
        {
            return mode == GameMode.Tag ? 2 : 1;
        }
        public static int PokemonsPerPlayer(this GameMode mode)
        {
            return mode == GameMode.Tag ? 3 : 6;
        }
        public static int OnboardPokemonsPerPlayer(this GameMode mode)
        {
            switch (mode)
            {
                case GameMode.Double:
                    return 2;
                case GameMode.Triple:
                case GameMode.Rotation:
                    return 3;
                default:
                    return 1;
            }
        }
        public static int GetPlayerIndex(this GameMode mode, int x)
        {
            return mode == GameMode.Tag ? x : 0;
        }
        public static int GetPokemonIndex(this GameMode mode, int x)
        {
            switch (mode)
            {
                case GameMode.Double:
                case GameMode.Triple:
                    return x;
                default:
                    return 0;
            }
        }
        public static bool NeedTarget(this GameMode mode)
        {
            return mode != GameMode.Single && mode != GameMode.Sky && mode != GameMode.Inverse;
        }
    }
}
