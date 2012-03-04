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
    internal event Action<int[]> RequireInput;
    internal event Action<Player> InputSucceed;
    HashSet<int> players;
    private Action InputFinished;

    public InputController(Controller controller)
      : base(controller)
    {
    }

    private void CheckInputSucceed(Player player)
    {
      foreach (Tile t in Controller.Tiles)
        if (t.ResponsiblePlayer == player)
          if (t.Pokemon != null)
          {
            if (t.Pokemon.Action == PokemonAction.WaitingForInput) return;
          }
          else
          {
            if (t.WillSendoutPokemon == null) return;
          }
      InputSucceed(player);
      players.Remove(player.Id);
      if (players.Count == 0 && InputFinished != null) InputFinished();
    }
    public void ContinueAfterInput(Action inputFinished)
    {
      players = new HashSet<int>();
      InputFinished = inputFinished;
      foreach (Tile t in Controller.Tiles)
        if (Controller.CanSendout(t) || t.Pokemon.Action != PokemonAction.WaitingForInput)
          players.Add(t.ResponsiblePlayer.Id);
      if (players.Count > 0) RequireInput(players.ToArray());
      else InputFinished();
    }
    public bool Switch(PokemonProxy withdraw, Pokemon sendout)
    {
      if (withdraw.Action == PokemonAction.WaitingForInput && withdraw.Pokemon.Owner == sendout.Owner)
        return withdraw.InputSwitch(sendout);
      return false;
    }
    /// <summary>
    /// 死亡交换或蜻蜓返，与原作不同同时挂两头也是依次立刻交换 〈— 真要这样做我就傻了
    /// </summary>
    /// <param name="sendout"></param>
    /// <param name="position"></param>
    /// <returns>succeed or not</returns>
    public bool Sendout(Pokemon sendout, Tile position)
    {
      if (Controller.CanSendout(sendout, position))
      {
        position.WillSendoutPokemon = sendout;
        CheckInputSucceed(position.ResponsiblePlayer);
        return true;
      }
      return false;
    }
    public bool SelectMove(MoveProxy move, Tile target)
    {
      if (move.Owner.Action == PokemonAction.WaitingForInput)
      {
        bool r = move.Owner.SelectMove(move, target);
        if (r) CheckInputSucceed(target.ResponsiblePlayer);
        return r;
      }
      return false;
    }
    public bool Struggle(PokemonProxy pokemon)
    {
      if (pokemon.Action == PokemonAction.WaitingForInput)
      {
        bool r = pokemon.SelectMove(pokemon.StruggleMove, null);
        if (r) CheckInputSucceed(pokemon.OnboardPokemon.Owner);
        return r;
      }
      return false;
    }
  }
}
