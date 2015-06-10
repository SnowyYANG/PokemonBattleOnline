using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokemonBattleOnline.Game.Host
{
    internal class Team
    {
        public readonly int Id;
        private readonly IGameSettings Settings;

        public Team(int id, Player[] players, IGameSettings settings)
        {
            Id = id;
            _players = players;
            Settings = settings;
            if (outward == null) outward = new BallState[Settings.Mode.PokemonsPerPlayer() * Settings.Mode.PlayersPerTeam()];
        }

        private readonly Player[] _players;
        public IEnumerable<Player> Players
        { get { return _players; } }

        public Player GetPlayer(int index)
        {
            return _players.ValueOrDefault(index);
        }

        private BallState[] outward;
        /// <summary>
        /// only once in a report fragment
        /// </summary>
        /// <returns></returns>
        public BallState[] GetOutward()
        {
            var ppp = Settings.Mode.PokemonsPerPlayer();
            int baseI = 0;
            foreach (var p in _players)
            {
                int i = baseI;
                foreach (var pm in p.Pokemons) outward[i++] = pm.Hp == 0 ? BallState.Faint : pm.State == PokemonState.Normal ? BallState.Normal : BallState.Abnormal;
                baseI += ppp;
            }
            return outward;
        }
    }
}
