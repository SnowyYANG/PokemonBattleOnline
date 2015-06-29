using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Game
{
    [KnownType("KnownEvents")]
    [DataContract(Namespace = PBOMarks.JSON)]
    public class ReportFragment
    {
        static Type[] knownGameEvents;
        static IEnumerable<Type> KnownEvents()
        {
            if (knownGameEvents == null) knownGameEvents = typeof(GameEvent).SubClasses();
            return knownGameEvents;
        }

        [DataMember(Name = "b", EmitDefaultValue = false)]
        private int _turnNumber;
        public int TurnNumber
        {
            get { return _turnNumber - 1; }
            private set { _turnNumber = value + 1; }
        }

        [DataMember(Name = "e", EmitDefaultValue = false)]
        public readonly Weather Weather;

        [DataMember(Name = "c_", EmitDefaultValue = false)]
        private readonly BallState[][] Players; //json序列化不支持多维数组
        [DataMember(Name = "d_", EmitDefaultValue = false)]
        private readonly PokemonOutward[] Pokemons; //在场精灵

        /// <summary>
        /// 为了节约流量，只在用户第一次进入房间的时候给出players/pms/weather信息
        /// </summary>
        public ReportFragment(int turnNumber, BallState[,][] players, PokemonOutward[] pms, Weather weather)
        {
            TurnNumber = turnNumber;
            Players = new BallState[players.Length][];
            Players[0] = players[0, 0];
            Players[1] = players[1, 0];
            if (Players.Length == 4)
            {
                Players[2] = players[0, 1];
                Players[3] = players[1, 1];
            }
            Pokemons = pms;
            Weather = weather;
            _events = new List<GameEvent>();
        }
        protected ReportFragment(ReportFragment fragment)
        {
            _turnNumber = fragment._turnNumber;
            Players = fragment.Players;
            Weather = fragment.Weather;
            Pokemons = fragment.Pokemons;
            _events = fragment._events;
        }
        private ReportFragment()
        {
        }

        private List<GameEvent> _events;
        [DataMember(Name = "a_", EmitDefaultValue = false)]
        private GameEvent[] e_
        {
            get { return _events.ToArray(); }
            set { _events = new List<GameEvent>(value); }
        }
        public IEnumerable<GameEvent> Events
        { get { return _events; } }

        public PokemonOutward GetPokemon(int team, int x)
        {
            PokemonOutward value = null;
            if (Pokemons != null)
                foreach (PokemonOutward p in Pokemons)
                    if (p.Position.Team == team && p.Position.X == x)
                    {
                        value = p;
                        break;
                    }
            return value;
        }

        public BallState[] GetPlayer(int team, int index)
        {
            return Players.ValueOrDefault(index * 2 + team);
        }

        /// <summary>
        /// Host使用
        /// </summary>
        /// <param name="e"></param>
        public void AddEvent(GameEvent e)
        {
            _events.Add(e);
        }
        public ReportFragment NonLeap()
        {
            return new ReportFragment() { _events = this._events };
        }
    }
}
