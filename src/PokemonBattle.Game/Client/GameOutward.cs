using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game
{
  /// <summary>
  /// 只能有一个Listner卡住线程，建议是Subtitle
  /// </summary>
  public interface IGameOutwardEvents
  {
    void TurnEnd();
    void GameLogAppend(IText t);
    void GameEnd();
  }
  public class GameOutward : IFormatProvider, ICustomFormatter
  {
    /// <summary>
    /// game start, or an observer
    /// </summary>
    public event Action LeapTurn;
    public readonly IGameSettings Settings;
    public readonly BoardOutward Board;
    public readonly TeamOutward[] Teams;
    private readonly Dictionary<int, PlayerOutward> players;
    private readonly string[] teams;
    private readonly Collection<IGameOutwardEvents> listeners;

    public GameOutward(IGameSettings settings, IDictionary<int, string> players, string[] teams)
    {
      Settings = settings;
      Board = new BoardOutward(Settings);
      Teams = new TeamOutward[Settings.Mode.TeamCount()];
      for (int t = 0; t < Settings.Mode.TeamCount(); t++)
        Teams[t] = new TeamOutward(6, 0, 0);
      {
        this.players = new Dictionary<int, PlayerOutward>();
        foreach (var p in players) this.players.Add(p.Key, new PlayerOutward(p.Key, p.Value));
      }
      this.teams = teams;
      listeners = new Collection<IGameOutwardEvents>();
    }
    public int TurnNumber
    { get; set; }

    public PlayerOutward GetPlayer(int id)
    {
      return players.ValueOrDefault(id);
    }
    public PokemonOutward GetPokemon(int id)
    {
      foreach (var team in Board.Teams)
        foreach (var pm in team)
          if (pm != null && pm.Id == id) return pm;
      return null;
    }
    public void Update(ReportFragment fragment)
    {
      if (fragment.Teams != null)
        UIDispatcher.Invoke(() =>
          {
            {
              AppendGameLog(GameHeader.I);
              var t = GameService.Logs["GameMode"].Clone(this);
              t.SetData(GameMode.Single);
              AppendGameLog(t);
              t = GameService.Logs["GameRule"].Clone(this);
              t.SetData("催眠条款");
              AppendGameLog(t);
            }
            if (fragment.TurnNumber > 0)
            {
              AppendGameLog(GameService.Logs["GameContinue"]);
              TurnNumber = fragment.TurnNumber;
            }
            for (int t = 0; t < Settings.Mode.TeamCount(); t++)
            {
              Teams[t].Update(fragment.Teams[t]);
              for (int x = 0; x < Settings.Mode.XBound(); x++) Board[t, x] = fragment[t, x];
              Board.Weather = fragment.Weather;
            }
            LeapTurn();
          });
      foreach (GameEvent e in fragment.Events)
      {
        UIDispatcher.Invoke((Action<GameOutward>)e.Update, this);
        System.Threading.Thread.Sleep(e.Sleep);
      }
      int team0 = Teams[0].Abnormal + Teams[0].Normal;
      int team1 = Teams[1].Abnormal + Teams[1].Normal;
      if (team0 == 0 || team1 == 0)
      {
        IText text;
        if (team0 == 0 && team1 == 0)
        {
          text = GameService.Logs["GameResultTie"].Clone(this);
          text.SetData(0, 1);
        }
        else
        {
          text = GameService.Logs["GameResult"].Clone(this);
          int winer = team0 == 0 ? 1 : 0;
          text.SetData(winer, team0, team1);
        }
        UIDispatcher.Invoke(() =>
          {
            AppendGameLog(text);
            GameEnd();
          });
      }
    }
    public void AppendGameLog(IText text)
    {
      foreach (var l in listeners) l.GameLogAppend(text);
    }
    public void EndTurn()
    {
      foreach (var l in listeners) l.TurnEnd();
    }
    public void GameEnd()
    {
      foreach (var l in listeners) l.GameEnd();
    }
    public void AddListner(IGameOutwardEvents listener)
    {
      if (listener != null) listeners.Add(listener);
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
        if (format == "e") r = DataService.String[arg.ToString()];
        else if (arg is int)
        {
          int id = (int)arg;
          switch (format)
          {
            case "p":
              {
                var pm = GetPokemon(id);
                if (pm != null) r = string.Format(this, DataService.String["{0}'s {1}"], pm.Owner.Name, pm.Name);
              }
              break;
            case "m":
              r = GameDataService.GetMove(id).GetLocalizedName();
              break;
            case "a":
              r = GameDataService.GetAbility(id).GetLocalizedName();
              break;
            case "i":
              r = GameDataService.GetItem(id).GetLocalizedName();
              break;
            case "t":
              r = teams[id];
              break;
            default:
              if (format != null && format.StartsWith("pm."))
              {
                var pm = GetPokemon(id);
                r = pm == null ? DataService.String["<error>"] : pm.GetProperty(format.Substring(3));
              }
              break;
          }//switch
        }
        if (r == null) r = arg.ToString();
      }// if (arg != null
      return r;
    }

    private class GameHeader : LogText
    {
      public static readonly GameHeader I = new GameHeader();
      
      private GameHeader()
        : base("POKEMON BATTLE ONLINE v0.8 <ETV>\n")
      {
        IsBold = true;
      }
    }
  }
}
