using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
    public bool Switch(PokemonProxy withdraw, Pokemon sendout)
    {
      if (withdraw.Action == PokemonAction.WaitingForInput && withdraw.Pokemon.Owner == sendout.Owner && sendout.Hp.Value > 0)
      {
        withdraw.Action = PokemonAction.Switch;
        withdraw.SwitchPokemon = sendout;
        return true;
      }
      return false;
    }
    /// <summary>
    /// 死亡交换或蜻蜓返
    /// </summary>
    /// <param name="sendout"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool Sendout(Pokemon sendout, Position position)
    {
      if (Controller.CanSendout(sendout, position))
      {
        return Controller.Sendout(sendout, position);
      }
      return false;
    }
    public bool SelectMove(MoveProxy move, Position position = null)
    {
      PokemonProxy pm = move.Owner;
      if (pm.Action == PokemonAction.WaitingForInput && pm.Hp > 0 && move.PP > 0)
      {
        #warning 锁技能
        pm.Action = PokemonAction.Moving;
        pm.SelectMove = move;
        pm.SelectTarget = position;
        return true;
      }
      return false;
    }
    public bool Struggle(PokemonProxy pokemon)
    {
      return false;
    }
  }
}
