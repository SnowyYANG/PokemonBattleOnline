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
    public event Action<ReportFragment> ReportUpdated;
    public event Action<int[]> RequireInput;
    public event Action<Player> InputSucceed;
    HashSet<int> players;
    private Action InputFinished;

    public InputController(Controller controller)
      : base(controller)
    {
    }

    private void CheckInputSucceed(Player player)
    {
      foreach (Tile t in Controller.Tiles)
        if (Controller.GetPlayer(t) == player)
          if (t.Pokemon != null)
          {
            if (t.Pokemon.Action == PokemonAction.WaitingForInput) return;
          }
          else
          {
            if (t.WillSendoutPokemonIndex == t.X) return;
          }
      players.Remove(player.Id);
      if (InputSucceed != null) InputSucceed(player);
      if (players.Count == 0 && InputFinished != null) InputFinished();
    }
    public void ContinueAfterInput(Action inputFinished)
    {
      players = new HashSet<int>();
      InputFinished = inputFinished;
      foreach (Tile t in Controller.Tiles)
        if (Controller.CanSendout(t) || t.Pokemon.Action == PokemonAction.WaitingForInput)
          players.Add(Controller.GetPlayer(t).Id);
      if (players.Count > 0)
      {
        ReportBuilder.NewFragment();
        if (ReportUpdated != null) ReportUpdated(ReportBuilder.GetFragment());
        if (RequireInput != null) RequireInput(players.ToArray());
      }
      else InputFinished();
    }
    public bool Switch(PokemonProxy withdraw, int sendoutIndex)
    {
      if (withdraw.UndoInput())
      {
        bool r = withdraw.InputSwitch(sendoutIndex);
        if (r) CheckInputSucceed(withdraw.Pokemon.Owner);
        return r;
      }
      return false;
    }
    public bool Sendout(Tile position, int sendoutIndex)
    {
      if (Controller.CanSendout(position) && Controller.CanSendout(Controller.GetPlayer(position).GetPokemon(sendoutIndex)))
      {
        position.WillSendoutPokemonIndex = sendoutIndex;
        CheckInputSucceed(Controller.GetPlayer(position));
        return true;
      }
      return false;
    }
    public bool SelectMove(MoveProxy move, Tile target)
    {
      if (move.Owner.UndoInput())
      {
        bool r = move.Owner.SelectMove(move, target);
        if (r) CheckInputSucceed(move.Owner.Pokemon.Owner);
        return r;
      }
      return false;
    }
    public bool Struggle(PokemonProxy pokemon)
    {
      pokemon.UndoInput();
      if (pokemon.Action == PokemonAction.WaitingForInput)
      {
        bool r = pokemon.SelectMove(pokemon.StruggleMove, null);
        if (r) CheckInputSucceed(pokemon.Pokemon.Owner);
        return r;
      }
      return false;
    }
  }
}
