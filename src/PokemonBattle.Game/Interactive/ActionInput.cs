using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.Tactic.DataModels;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Interactive
{
  public class InputResult
  {
    public static InputResult Succeed(bool allDone)
    {
      return new InputResult(true, null, allDone);
    }
    public static InputResult Fail(string messageKey = null)
    {
      return new InputResult(false, messageKey, false);
    }

    public readonly bool IsSucceeded;
    public readonly bool AllDone;
    public readonly string MessageKey;

    private InputResult(bool succeed, string messageKey, bool allDone)
    {
      IsSucceeded = succeed;
      AllDone = allDone;
      MessageKey = messageKey;
    }
  }
  
  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class ActionInput
  {
    public static ActionInput UseMove(byte x, SimMove move, Tile target)
    {
      ActionInput i = new ActionInput(x) { Move = move.Type.Id };
      if (target != null)
      { 
        i.TargetTeam = (byte)(target.Team + 1);
        i.TargetX = (byte)(target.X + 1);
      }
      return i;
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

    internal InputResult Input(Controller controller, Player player)
    {
      InputResult r = InputResult.Fail();
      if (controller.Game.Settings.Mode.GetPlayerIndex(X) == player.Team.GetPlayerIndex(player.Id))
      {
        Tile tile = controller.GetTile(player.TeamId, X);
        if (SendoutIndex > 0) r = controller.InputSendout(tile, SendoutIndex);
        else
        {
          var pm = tile.Pokemon;
          if (Move > 0)
          {
            foreach (MoveProxy m in pm.Moves)
              if (m != null && m.Type.Id == Move)
              {
                r = controller.InputSelectMove(m, controller.GetTile(TargetTeam, TargetX));
                break;
              }
          }
          else r = controller.InputStruggle(pm);
        }
      }
      return r;
    }
  }
}
