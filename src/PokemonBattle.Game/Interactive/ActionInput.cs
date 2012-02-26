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
    internal static bool InputAction(Controller game, Player player, ActionInput input)
    {
      
      return true;
    }
    public static ActionInput UseMoveAction(Move move, Position target)
    {
      return new ActionInput(move.Id) { Target = target };
    }
    public static ActionInput SwitchPokemonAction(SimPokemon withdraw, Pokemon sendout)
    {
      return new ActionInput(withdraw.SwitchId) { SendoutId = sendout.Id };
    }
    public static ActionInput Struggle(SimPokemon pm)
    {
      return new ActionInput(pm.StruggleId);
    }

    [DataMember]
    int ActionId;

    [DataMember(EmitDefaultValue = false)]
    Position Target;

    [DataMember(EmitDefaultValue = false)]
    int SendoutId;

    private ActionInput(int actionId)
    {
      this.ActionId = actionId;
    }
  }
}
