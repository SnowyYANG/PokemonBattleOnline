using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.Tactic.DataModels;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Interactive
{
  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class ActionInput
  {
    public static ActionInput UseMove(Move move, Position target)
    {
      return new ActionInput(move.Id) { Position = target };
    }
    public static ActionInput Switch(SimPokemon withdraw, Pokemon sendout)
    {
      return new ActionInput(withdraw.SwitchId) { SendoutId = sendout.Id };
    }
    public static ActionInput Sendout(Pokemon sendout, Position position)
    {
      return new ActionInput(0) { SendoutId = sendout.Id, Position = position };
    }
    public static ActionInput Struggle(SimPokemon pm)
    {
      return new ActionInput(pm.StruggleId);
    }

    [DataMember]
    int ActionId;

    [DataMember(EmitDefaultValue = false)]
    Position Position;

    [DataMember(EmitDefaultValue = false)]
    int SendoutId;

    private ActionInput(int actionId)
    {
      this.ActionId = actionId;
    }

    /// <summary>
    /// 只判断pm归属权问题
    /// </summary>
    internal bool Input(Controller controller, Player player)
    {
      if (SendoutId != 0)
      {
        Pokemon sendout = controller.Game.GetPokemon(SendoutId);
        if (sendout.Owner == player)
        {
          if (ActionId == 0) return controller.InputSendout(sendout, Position);
          foreach (PokemonProxy p in controller.OnboardPokemons)
            if (p.Pokemon.SwitchId == ActionId && p.Pokemon.Owner == player)
              return controller.InputSwitch(p, sendout);
        }
      }
      else
      {
        foreach (PokemonProxy p in controller.OnboardPokemons)
          if (p.Pokemon.Owner == player)
          {
            if (p.Pokemon.StruggleId == ActionId) controller.InputStruggle(p);
            foreach (MoveProxy m in p.Moves)
              controller.InputSelectMove(m, Position);
          }
      }
      return false;
    }
  }
}
