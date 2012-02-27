using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Interactive;

namespace LightStudio.PokemonBattle.Game
{
  internal class InputController : ControllerComponent
  {
    private event Action InputFinished;

    public InputController(Controller controller)
      : base(controller)
    {
    }

    private void CheckForInputFinished()
    {
      foreach (PokemonProxy p in Controller.OnboardPokemons)
        if (p.Action == PokemonAction.WaitingForInput) return;
      if (InputFinished != null) InputFinished();
    }
    public ActionInputFailure Switch(PokemonProxy withdraw, Pokemon sendout)
    {
      if (withdraw.Action == PokemonAction.WaitingForInput && withdraw.Pokemon.Owner == sendout.Owner)
        return withdraw.InputSwitch(sendout);
      return new OtherActionInputFailure("InputController");
    }
    /// <summary>
    /// 死亡交换或蜻蜓返，与原作不同同时挂两头也是依次立刻交换
    /// </summary>
    /// <param name="sendout"></param>
    /// <param name="position"></param>
    /// <returns>succeed or not</returns>
    public ActionInputFailure Sendout(Pokemon sendout, Position position)
    {
      if (Controller.Sendout(sendout, position)) return null;
      return new OtherActionInputFailure("InputController");
    }
    public ActionInputFailure SelectMove(MoveProxy move, Position position)
    {
      if (move.Owner.Action == PokemonAction.WaitingForInput)
        return move.Owner.SelectMove(move, position);
      return new OtherActionInputFailure("InputController");
    }
    public ActionInputFailure Struggle(PokemonProxy pokemon)
    {
      if (pokemon.Action == PokemonAction.WaitingForInput)
        return pokemon.SelectMove(pokemon.StruggleMove, null);
      return new OtherActionInputFailure("InputController");
    }
  }
}
