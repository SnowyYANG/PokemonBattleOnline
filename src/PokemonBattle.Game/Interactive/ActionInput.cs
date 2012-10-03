using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.Tactic.DataModels;
using LightStudio.PokemonBattle.Game.Host;

namespace LightStudio.PokemonBattle.Game
{
  [DataContract(Namespace = Namespaces.PBO)]
  internal class XActionInput
  {
    public static XActionInput UseMove(SimMove move, int targetTeam, int targetX)
    {
      return new XActionInput() { Move = move.Type.Id, TargetTeam = (byte)(targetTeam + 1), TargetX = (byte)(targetX + 1) };
    }
    public static XActionInput UseMove(SimMove move)
    {
      return new XActionInput() { Move = move.Type.Id };
    }
    public static XActionInput Sendout(Pokemon sendout)
    {
      return new XActionInput() { SendoutIndex = (byte)sendout.IndexInOwner };
    }
    public static XActionInput Struggle()
    {
      return new XActionInput();
    }

    [DataMember(EmitDefaultValue = false)]
    int Move;

    [DataMember(EmitDefaultValue = false)]
    byte TargetTeam;

    [DataMember(EmitDefaultValue = false)]
    byte TargetX;

    [DataMember(EmitDefaultValue = false)]
    byte SendoutIndex;

    private XActionInput()
    {
    }

    public bool Input(Controller controller, Tile tile)
    {
      bool r = false;
      if (SendoutIndex > 0) r = controller.InputSendout(tile, SendoutIndex);
      else
      {
        var pm = tile.Pokemon;
        if (Move > 0)
        {
          foreach (MoveProxy m in pm.Moves)
            if (m.Type.Id == Move)
            {
              Tile target = TargetTeam > 0 ? controller.GetTile(TargetTeam - 1, TargetX - 1) : null;
              r = controller.InputSelectMove(m, target);
              break;
            }
        }
        else r = controller.InputStruggle(pm);
      }
      return r;
    }
  }
  [DataContract(Namespace = Namespaces.PBO)]
  public class ActionInput
  {
    [DataMember]
    XActionInput[] inputs;

    public ActionInput(int maxX)
    {
      inputs = new XActionInput[maxX];
    }

    public void UseMove(int x, SimMove move, int targetTeam, int targetX)
    {
      inputs[x] = XActionInput.UseMove(move, targetTeam, targetX);
    }
    public void UseMove(int x, SimMove move)
    {
      inputs[x] = XActionInput.UseMove(move);
    }
    public void Switch(int x, Pokemon sendout)
    {
      inputs[x] = XActionInput.Sendout(sendout);
    }
    public void Sendout(int x, Pokemon sendout)
    {
      inputs[x] = XActionInput.Sendout(sendout);
    }
    public void Struggle(int x)
    {
      inputs[x] = XActionInput.Struggle();
    }

    internal bool Input(Controller controller, Player player)
    {
      for (int x = 0; x < inputs.Length; ++x)
        if (inputs[x] != null)
        { 
          if (controller.Game.Settings.Mode.GetPlayerIndex(x) != player.Team.GetPlayerIndex(player.Id)) return false;
          if (!inputs[x].Input(controller, controller.GetTile(player.TeamId, x))) return false;
        }
      return controller.CheckInputSucceed(player);
    }
  }
}
