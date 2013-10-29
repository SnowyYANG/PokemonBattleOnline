using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using PokemonBattleOnline.Game;

namespace PokemonBattleOnline.Game
{
  /// <summary>
  /// 只能有一个Listner卡住线程，建议是Subtitle
  /// </summary>
  public interface IGameOutwardEvents
  {
    void TurnEnd();
    void GameLogAppend(LogText t);
  }
  public class GameOutward : IFormatProvider, ICustomFormatter
  {
    /// <summary>
    /// game start, or an observer
    /// </summary>
    public event Action GameStart;
    public event Action GameEnd;
    public readonly IGameSettings Settings;
    public readonly BoardOutward Board;
    public readonly TeamOutward[] Teams;
    private readonly string[, ] Players;
    private readonly Collection<IGameOutwardEvents> listeners;

    public GameOutward(IGameSettings settings, string[,] players)
    {
      Settings = settings;
      Board = new BoardOutward(Settings);
      Teams = new TeamOutward[Settings.Mode.TeamCount()];
      Players = players;
      listeners = new Collection<IGameOutwardEvents>();
    }
    public int TurnNumber
    { get; set; }

    public string GetPlayerName(int team, int index)
    {
      return Players[team, index];
    }
    public PokemonOutward GetPokemon(int id)
    {
      foreach (var team in Board.Teams)
        foreach (var pm in team)
          if (pm != null && pm.Id == id) return pm;
      return null;
    }
    public void Start(ReportFragment fragment)
    {
      {
        AppendGameLog(GameHeader.I);
        var t = GameLogs.Log("GameMode").Clone(this);
        t.SetData(Settings.Mode == GameMode.Single ? "单打" : Settings.Mode == GameMode.Tag ? "合作" : Settings.Mode.ToString());
        AppendGameLog(t);
        t = GameLogs.Log("GameRule").Clone(this);
        t.SetData("催眠条款");
        AppendGameLog(t);
      }
      if (fragment.TurnNumber >= 0) AppendGameLog(GameLogs.Log("GameContinue"));
      TurnNumber = fragment.TurnNumber;
      for (int t = 0; t < Settings.Mode.TeamCount(); t++)
      {
        Teams[t] = new TeamOutward(Players[t, 0], fragment.Teams[t]);
        for (int x = 0; x < Settings.Mode.XBound(); x++) Board[t, x] = fragment[t, x];
        Board.Weather = fragment.Weather;
      }
      GameStart();
    }
    public void Update(IEnumerable<GameEvent> events)
    {
      foreach (GameEvent e in events)
        UIDispatcher.Invoke((Action<GameOutward>)e.Update, this);
      //check game over
      int team0 = Teams[0].AliveCount;
      int team1 = Teams[1].AliveCount;
      if (team0 == 0 || team1 == 0)
      {
        LogText text;
        if (team0 == 0 && team1 == 0)
        {
          text = GameLogs.Log("GameResultTie").Clone(this);
          text.SetData(0, 1);
        }
        else
        {
          text = GameLogs.Log("GameResult").Clone(this);
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
    public void AppendGameLog(LogText text)
    {
      foreach (var l in listeners) l.GameLogAppend(text);
    }
    public void EndTurn()
    {
      foreach (var l in listeners) l.TurnEnd();
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
        var type = arg.GetType();
        if (type == typeof(BattleType)) r = GameString.Current.BattleType((BattleType)arg);
        else if (type == typeof(int))
        {
          int id = (int)arg;
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
            case "t":
              r = Teams[id].Name;
              break;
            default:
              if (format != null && format.StartsWith("pm."))
              {
                var pm = GetPokemon(id);
                r = pm == null ? "<error>" : pm.GetProperty(format.Substring(3));
              }
              break;
          }//switch
        }
        else r = GameString.Current.BattleLog(arg.ToString());
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
