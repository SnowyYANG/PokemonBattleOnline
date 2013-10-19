using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PokemonBattleOnline.Game.Host
{
  public class GoTimer : IDisposable
  {
    public event Action<IEnumerable<KeyValuePair<int, int>>> TimeUp;
    public event Action<IEnumerable<int>> WaitingNotify;
    private readonly Timer timer;
    private readonly Dictionary<int, Player> players;
    private readonly object Locker;

    internal GoTimer(IEnumerable<int> players)
    {
      Locker = new object();
      timer = new Timer(TimeTick, null, Timeout.Infinite, 1000);
      this.players = new Dictionary<int, Player>();
      foreach (int p in players) this.players.Add(p, new Player());
    }

    public void Start()
    {
      timer.Change(0, 1000);
    }
    public int GetState(int player)
    {
      lock (Locker)
      {
        return players[player].SpentTime;
      }
    }

    private int require;
    private int done;
    private int reminderCount;
    public void Resume(IEnumerable<int> players)
    {
      lock (Locker)
      {
        require = players.Count();
        done = 0;
        reminderCount = require == 1 ? 30 : 0;
        foreach (int p in players) this.players[p].Timing = true;
      }
    }
    public void Pause(int player)
    {
      lock (Locker)
      {
        if (players[player].Timing)
        {
          players[player].Timing = false;
          done++;
          reminderCount = require == done ? 0 : 30;
        }
      }
    }
    public void NewTurn()
    {
      lock (Locker)
      {
        foreach (Player p in players.Values) p.SpentTime -= 30;
      }
    }
    private void TimeTick(object state)
    {
      lock (Locker)
      {
        bool timeup = false;
        foreach (Player p in players.Values)
          if (p.Timing)
          {
            p.SpentTime++;
            timeup |= p.SpentTime > PBOMarks.GAMETIMEUP;
          }
        if (timeup)
        {
          timer.Change(Timeout.Infinite, 0);
          TimeUp(from p in players select new KeyValuePair<int, int>(p.Key, p.Value.SpentTime));
        }
        else if (reminderCount > 0)
        {
          reminderCount--;
          if (reminderCount == 0) WaitingNotify(from p in players where p.Value.Timing select p.Key);
        }
      }
    }

    public void Dispose()
    {
      timer.Dispose();
    }

    private class Player
    {
      public bool Timing;
      public int SpentTime;
    }
  }
}
