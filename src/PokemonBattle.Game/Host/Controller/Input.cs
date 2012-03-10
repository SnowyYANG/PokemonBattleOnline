using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Interactive;

namespace LightStudio.PokemonBattle.Game
{
  /// <summary>
  /// 我记得这是单线程的，Host那边用了Dispatcher
  /// </summary>
  internal class InputController : ControllerComponent
  {
    public event Action<ReportFragment, int[]> ReportUpdated;
    private HashSet<int> players;

    public InputController(Controller controller)
      : base(controller)
    {
    }

    private InputResult CheckInputSucceed(Player player)
    {
      foreach (Tile t in Controller.Tiles)
        if (Controller.GetPlayer(t) == player)
          if (t.Pokemon != null)
          {
            if (t.Pokemon.Action == PokemonAction.WaitingForInput) return InputResult.Succeed();
          }
          else
          {
            if (t.WillSendoutPokemonIndex == t.X) return InputResult.Succeed();
          }
      players.Remove(player.Id);
      return InputResult.Succeed(true);
    }
    public bool PauseForInput()
    {
      players = new HashSet<int>();
      foreach (Tile t in Controller.Tiles)
        if (Controller.CanSendout(t) || t.Pokemon.Action == PokemonAction.WaitingForInput)
          players.Add(Controller.GetPlayer(t).Id);
      if (players.Count > 0)
      {
        ReportBuilder.NewFragment();
        if (ReportUpdated != null) ReportUpdated(ReportBuilder.GetFragment(), players.ToArray());
        return true;
      }
      return false;
    }
    public InputResult Switch(PokemonProxy withdraw, int sendoutIndex)
    {
      if (withdraw.UndoInput())
      {
        string m = withdraw.InputSwitch(sendoutIndex);
        if (m == null) return CheckInputSucceed(withdraw.Pokemon.Owner);
        return InputResult.Fail(m == string.Empty ? null : m);
      }
      return InputResult.Fail("当前精灵没有等待训练师的命令");
    }
    public InputResult Sendout(Tile tile, int sendoutIndex)
    {
      if (!Controller.CanSendout(tile)) return InputResult.Fail("非空场地或已经没有可以送出的精灵了");
      if (!Controller.CanSendout(Controller.GetPlayer(tile).GetPokemon(sendoutIndex))) return InputResult.Fail("这只精灵无法被送出");
      tile.WillSendoutPokemonIndex = sendoutIndex;
      return CheckInputSucceed(Controller.GetPlayer(tile));
    }
    public InputResult SelectMove(MoveProxy move, Tile target)
    {
      if (move.Owner.UndoInput())
      {
        string m = move.Owner.SelectMove(move, target);
        if (m == null) return CheckInputSucceed(move.Owner.Pokemon.Owner);
        return InputResult.Fail(m == string.Empty ? null : m);
      }
      return InputResult.Fail("当前精灵没有等待训练师的命令");
    }
    public InputResult Struggle(PokemonProxy pokemon)
    {
      if (pokemon.UndoInput())
      {
        string m = pokemon.SelectMove(pokemon.StruggleMove, null);
        if (m == null) return CheckInputSucceed(pokemon.Pokemon.Owner);
        return InputResult.Fail(m == string.Empty ? null : m);
      }
      return InputResult.Fail("当前精灵没有等待训练师的命令");
    }
  }
}
