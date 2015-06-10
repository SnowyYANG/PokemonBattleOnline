using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace PokemonBattleOnline.Game
{
    public class GameOutward : IFormatProvider, ICustomFormatter
    {
        /// <summary>
        /// game start, or an observer
        /// </summary>
        public event Action<Exception> Error;
        public event Action GameStart;
        public event Action GameEnd;
        public event Action<string, LogStyle> LogAppended;
        public event Action TurnEnd;
        public readonly IGameSettings Settings;
        public readonly BoardOutward Board;
        private readonly string[,] Players;

        public GameOutward(IGameSettings settings, string[,] players)
        {
            Settings = settings;
            Players = players;
            Board = new BoardOutward(Settings);
            Board.Teams[0] = new TeamOutward(players[0, 0]);
            Board.Teams[1] = new TeamOutward(players[1, 0]);
#if TEST
            LogAppended = delegate { };
            TurnEnd = delegate { };
#endif
        }
        public int TurnNumber
        { get; set; }

        public string GetPlayerName(int team, int index)
        {
            return Players[team, index];
        }
        public PokemonOutward GetPokemon(int id)
        {
            foreach (var team in Board.Pokemons)
                foreach (var pm in team)
                    if (pm != null && pm.Id == id) return pm;
            return null;
        }
        public void Start(ReportFragment fragment)
        {
            LogAppended(PBOMarks.TITLE, LogStyle.Bold);
            AppendGameLog("GameMode", LogStyle.Default, Settings.Mode == GameMode.Single ? "单打" : Settings.Mode == GameMode.Tag ? "合作" : Settings.Mode.ToString());
            if (Settings.SleepRule) AppendGameLog("GameRule", LogStyle.Default, "催眠条款");
            if (fragment.TurnNumber >= 0) AppendGameLog("GameContinue", LogStyle.Default);
            TurnNumber = fragment.TurnNumber;
            for (int t = 0; t < 2; t++)
            {
                Board.Teams[t].SetAll(fragment.Teams[t]);
                for (int x = 0; x < Settings.Mode.XBound(); x++) Board[t, x] = fragment[t, x];
                Board.Weather = fragment.Weather;
            }
            UIDispatcher.Invoke(GameStart);
        }
        public void Update(IEnumerable<GameEvent> events)
        {
            foreach (GameEvent e in events)
            {
                try
                {
                    UIDispatcher.Invoke((Action<GameOutward>)e.Update, this);
                }
                catch (Exception ex)
                {
                    UIDispatcher.Invoke(Error, ex);
                }
#if !TEST
                System.Threading.Thread.Sleep(e.Sleep);
#endif
            }
        }
        public void EndGame()
        {
            UIDispatcher.Invoke(GameEnd);
        }

        public void AppendGameLog(string key, LogStyle style, object arg0 = null, object arg1 = null, object arg2 = null)
        {
            var log = GameString.Current.BattleLog(key);
            if (log != null) LogAppended(string.Format(this, log, arg0, arg1, arg2), style);
            else if (key != LogKeys.NO_KEY) AppendGameLog(LogKeys.NO_KEY, LogStyle.SYS, key);
        }
        public void EndTurn()
        {
            TurnEnd();
        }

        object IFormatProvider.GetFormat(Type formatType)
        {
            if (formatType == typeof(ICustomFormatter)) return this;
            else return null;
        }
        string ICustomFormatter.Format(string format, object arg, IFormatProvider formatProvider)
        {
            string r = null;
            if (arg != null)
            {
                var type = arg.GetType();
                if (type == typeof(BattleType)) r = GameString.Current.BattleType((BattleType)arg);
                else if (type == typeof(int))
                {
                    int id = (int)arg;
                    if (format == null) r = id.ToString();
                    else
                    {
                        switch (format)
                        {
                            case "p":
                                {
                                    var pm = GetPokemon(id);
                                    if (pm != null) r = string.Format(GameString.Current.BattleLog("OwnersPokemon"), pm.Owner, pm.Name);
                                }
                                break;
                            case "m":
                                r = GameString.Current.Move(id);
                                break;
                            case "a":
                                r = GameString.Current.Ability(id);
                                break;
                            case "i":
                                r = GameString.Current.Item(id);
                                break;
                            case "b":
                                r = GameString.Current.BattleType((BattleType)id);
                                break;
                            case "s":
                                r = GameString.Current.StatType((StatType)id);
                                break;
                            case "t":
                                var t = Board.Teams.ValueOrDefault(id);
                                if (t != null) r = t.Name;
                                break;
                            default:
                                if (format.StartsWith("pm."))
                                {
                                    var pm = GetPokemon(id);
                                    if (pm != null) r = pm.GetProperty(format.Substring(3));
                                }
                                break;
                        }//switch
                        if (r == null) r = "<error>";
                    }
                }
                else r = GameString.Current.BattleLog(arg.ToString());
            }// if (arg != null
            return r;
        }
    }
}
