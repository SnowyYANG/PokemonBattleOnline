using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokemonBattleOnline.Game.GameEvents;

namespace PokemonBattleOnline.Game.Host
{
  internal class ReportBuilder
  {
    private readonly Controller Controller;
    private readonly DateTime Begin;
    private ReportFragment lastLeapFragment;
    private ReportFragment lastFragment;
    private ReportFragment current;

    internal ReportBuilder(Controller controller)
    {
      Controller = controller;
      TurnNumber = -1;
      Begin = DateTime.Now;
    }

    public int TurnNumber
    { get; private set; }

    internal void NewFragment()
    {
      if (current != null)
      {
        lastLeapFragment = current;
        if (lastFragment == null) lastFragment = lastLeapFragment;
        else lastFragment = new ReportFragment(lastLeapFragment);
      }

      TeamOutward[] t = new TeamOutward[Controller.Board.TeamCount];
      for (int i = 0; i < t.Length; i++) t[i] = Controller.Game.Teams[i].GetOutward();
      List<PokemonOutward> pms = new List<PokemonOutward>();
      {
        foreach (PokemonProxy p in Controller.ActingPokemons) pms.Add(p.GetOutward());
        current = new ReportFragment(Controller.TurnNumber, t, pms.ToArray(), Controller.Board.Weather);
        var s = (int)((DateTime.Now - Begin).TotalSeconds);
        if (s != 0) Add(new TimeTick(s));
      }
    }
    internal ReportFragment GetFragment()
    {
      return lastFragment;
    }
    internal ReportFragment GetLeapFragment()
    {
      return lastLeapFragment;
    }
    internal void NewTurn()
    {
      ++TurnNumber;
      Add(new BeginTurn());
    }

    public void Add(GameEvent e)
    {
      current.AddEvent(e);
    }
    private static object Filter(object o)
    {
      object r = null;
      if (o != null)
      {
        if (o is PokemonProxy) r = ((PokemonProxy)o).Id;
        else if (o is Item) r = ((Item)o).Id;
        else if (o is int) r = (int)o;
#if DEBUG
        else if (o is Enum) r = o.ToString();
        else if (o is string) r = (string)o;
        else throw new Exception("bad event arg");
#else
        else r = o.ToString();
#endif
      }
      return r;
    }
    public void ShowLog(string key, object arg0 = null, object arg1 = null, object arg2 = null)
    {
      Add(new ShowLog(key, Filter(arg0), Filter(arg1), Filter(arg2)));
    }
    public void AddHorizontalLine()
    {
      if (!(current.LastEvent is HorizontalLine)) current.AddEvent(new HorizontalLine());
    }
    public void Mimic(PokemonProxy pm, MoveType move)
    {
      Add(new Mimic() { Pm = pm.Id, Move = move.Id });
    }
    public void SetPP(MoveProxy move)
    {
      Add(new SetPP() { Pm = move.Owner.Id, Move = move.Type.Id, PP = move.PP });
    }
    public void SetItem(PokemonProxy pm)
    {
      Add(new SetItem() { Pm = pm.Id, Item = pm.Pokemon.Item.Id });
    }
    public void SetHp(Pokemon pm)
    {
      Add(new SetHp() { Pm = pm.Id, Hp = pm.Hp.Value });
    }
    public void ShowHp(PokemonProxy pm)
    {
      Add(new ShowHp() { Pm = pm.Id, Hp = pm.Hp });
    }
    public void SetState(Pokemon pm)
    {
      Add(new SetState() { Pm = pm.Id, State = pm.State });
    }
    public void Transform(PokemonProxy pm)
    {
      var o = pm.GetOutward();
      Add(new SetOutward() { Pm = pm.Id, Number = o.Form.Species.Number, Form = o.Form.Index, Moves = pm.Moves.Select((m) => m.Type.Id).ToArray() });
    }
    public void DeIllusion(PokemonProxy pm)
    {
      var o = pm.GetOutward();
      Add(new SetOutward() { Pm = pm.Id, Number = o.Form.Species.Number, Form = o.Form.Index, Name = o.Name, Gender = o.Gender });
    }
    public void ChangeForm(PokemonProxy pm)
    {
      Add(new SetOutward() { Pm = pm.Id, Form = pm.OnboardPokemon.Form.Index, Forever = pm.Pokemon.Form.Species.Number == 492 && pm.OnboardPokemon.Form == pm.Pokemon.Form });
    }
    public void EnSubstitute(PokemonProxy pm)
    {
      Add(new Substitute() { Pm = pm.Id });
    }
    public void DeSubstitute(PokemonProxy pm)
    {
      Add(new Substitute() { Pm = pm.Id, De = true });
    }
    public void SendOut(PokemonProxy pm, int formerIndex)
    {
      Add(new SendOut() { Pm = pm.GetOutward(), FormerIndex = formerIndex });
    }
    public void Withdraw(PokemonProxy pm)
    {
      Add(new Withdraw() { Pm = pm.Id });
    }
    public void ShowWeather(Controller controller)
    {
      Add(new ShowWeather(controller.Board.Weather));
    }
    public void SetY(PokemonProxy pm)
    {
      Add(new SetY() { Pm = pm.Id, Y = pm.CoordY });
    }
    public void SetX(PokemonProxy pm)
    {
      Add(new SetX() { Pm = pm.Id, X = pm.OnboardPokemon.X });
    }
  }
}
