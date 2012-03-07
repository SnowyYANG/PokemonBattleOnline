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
    public static ActionInput UseMove(SimMove move, Tile target)
    {
      return new ActionInput(move.Move.Id) { TargetTeam = target.Team, TargetX = target.X };
    }
    public static ActionInput Switch(SimPokemon withdraw, Pokemon sendout)
    {
      return new ActionInput(withdraw.SwitchId) { SendoutIndex = sendout.IndexInOwner };
    }
    public static ActionInput Sendout(Pokemon sendout, Tile target)
    {
      return new ActionInput(0) { SendoutIndex = sendout.IndexInOwner, TargetTeam = target.Team, TargetX = target.X };
    }
    public static ActionInput Struggle(SimPokemon pm)
    {
      return new ActionInput(pm.StruggleId);
    }

    [DataMember]
    int ActionId;

    [DataMember(EmitDefaultValue = false)]
    int TargetTeam = -1;

    [DataMember(EmitDefaultValue = false)]
    int TargetX = -1;

    [DataMember(EmitDefaultValue = false)]
    int SendoutIndex;

    private ActionInput(int actionId)
    {
      this.ActionId = actionId;
    }

    /// <summary>
    /// 只判断pm归属权问题
    /// </summary>
    internal bool Input(Controller controller, Player player)
    {
      if (SendoutIndex != 0) //虽然用-1更好，不过0也不会错，还省流量
      {
        if (ActionId == 0)
          return controller.InputSendout(controller.GetTile(TargetTeam, TargetX), SendoutIndex);
        foreach (PokemonProxy p in controller.OnboardPokemons)
          if (p.Pokemon.SwitchId == ActionId && p.Pokemon.Owner == player)
            return controller.InputSwitch(p, SendoutIndex);
      }
      else
      {
        foreach (PokemonProxy p in controller.OnboardPokemons)
          if (p.Pokemon.Owner == player)
          {
            if (p.Pokemon.StruggleId == ActionId) controller.InputStruggle(p);
            foreach (MoveProxy m in p.Moves)
              controller.InputSelectMove(m, controller.GetTile(TargetTeam, TargetX));
          }
      }
      return false;
    }
  }
}
