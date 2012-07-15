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
      players = new HashSet<int>();
    }

    public bool NeedInput
    { get { return players.Count > 0; } }
    public IEnumerable<int> Players
    { get { return players; } }

    private InputResult CheckInputSucceed(Player player)
    {
      foreach (Tile t in Controller.Tiles)
        if (Controller.GetPlayer(t) == player)
          if (t.Pokemon != null)
          {
            if (t.Pokemon.Action == PokemonAction.WaitingForInput) return InputResult.Succeed(false);
          }
          else
          {
            if (Controller.CanSendout(t)) return InputResult.Succeed(false);
          }
      players.Remove(player.Id);
      return InputResult.Succeed(true);
    }
    public void PauseForTurnInput()
    {
      foreach (PokemonProxy p in Controller.OnboardPokemons)
        if (p.Action == PokemonAction.WaitingForInput) players.Add(p.Pokemon.Owner.Id);
    }
    public void PauseForEndTurnInput()
    {
      foreach (Tile t in Controller.Tiles)
        if (Controller.CanSendout(t)) players.Add(Controller.GetPlayer(t).Id);
    }
    public void PauseForSendoutInput(Tile tile)
    {
      if (Controller.CanSendout(tile))
          players.Add(Controller.GetPlayer(tile).Id);
    }
    private InputResult Switch(PokemonProxy withdraw, int sendoutIndex)
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
      if (tile.Pokemon == null)
      {
        if (!Controller.CanSendout(tile)) return InputResult.Fail("非空场地或已经没有可以送出的精灵了");
        if (!Controller.CanSendout(Controller.GetPlayer(tile).GetPokemon(sendoutIndex))) return InputResult.Fail("这只精灵无法被送出");
        tile.WillSendoutPokemonIndex = sendoutIndex;
        return CheckInputSucceed(Controller.GetPlayer(tile));
      }
      else return Switch(tile.Pokemon, sendoutIndex);
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
