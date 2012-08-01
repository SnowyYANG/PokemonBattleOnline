using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.Tactic.DataModels;
using LightStudio.PokemonBattle.Game.Host;

namespace LightStudio.PokemonBattle.Game
{
  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class ActionInput
  {
    public static ActionInput UseMove(byte x, SimMove move, int targetTeam, int targetX)
    {
      return new ActionInput(x) { Move = move.Type.Id, TargetTeam = (byte)(targetTeam + 1), TargetX = (byte)(targetX + 1) };
    }
    public static ActionInput UseMove(byte x, SimMove move)
    {
      return new ActionInput(x) { Move = move.Type.Id };
    }
    public static ActionInput Switch(byte x, Pokemon sendout)
    {
      return new ActionInput(x) { SendoutIndex = (byte)sendout.IndexInOwner };
    }
    public static ActionInput Sendout(byte x, Pokemon sendout, Tile target)
    {
      return new ActionInput(x) { SendoutIndex = (byte)sendout.IndexInOwner, TargetTeam = (byte)target.Team, TargetX = (byte)target.X };
    }
    public static ActionInput Struggle(byte x)
    {
      return new ActionInput(x);
    }

    [DataMember(EmitDefaultValue = false)]
    byte X;

    [DataMember(EmitDefaultValue = false)]
    int Move;

    [DataMember(EmitDefaultValue = false)]
    byte TargetTeam;

    [DataMember(EmitDefaultValue = false)]
    byte TargetX;

    [DataMember(EmitDefaultValue = false)]
    byte SendoutIndex;

    private ActionInput(byte x)
    {
      X = x;
    }

    internal bool Input(Controller controller, Player player)
    {
      throw new NotImplementedException();
      //InputResult r = InputResult.Fail();
      //if (controller.Game.Settings.Mode.GetPlayerIndex(X) == player.Team.GetPlayerIndex(player.Id))
      //{
      //  Tile tile = controller.GetTile(player.TeamId, X);
      //  if (SendoutIndex > 0) r = controller.InputSendout(tile, SendoutIndex);
      //  else
      //  {
      //    var pm = tile.Pokemon;
      //    if (Move > 0)
      //    {
      //      foreach (MoveProxy m in pm.Moves)
      //        if (m.Type.Id == Move)
      //        {
      //          Tile target = TargetTeam > 0 ? controller.GetTile(TargetTeam - 1, TargetX - 1) : null;
      //          r = controller.InputSelectMove(m, target);
      //          break;
      //        }
      //    }
      //    else r = controller.InputStruggle(pm);
      //  }
      //}
      //return r;
    }
  }
}
