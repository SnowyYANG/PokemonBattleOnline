using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Game
{
  [DataContract(Namespace = PBOMarks.JSON)]
  public class XActionInput
  {
    public static XActionInput UseMove(SimMove move, int targetTeam, int targetX)
    {
      return new XActionInput(move.Type.Id, targetTeam + 1, targetX + 1, 0);
    }
    public static XActionInput UseMove(SimMove move)
    {
      return new XActionInput(move.Type.Id, 0, 0, 0);
    }
    public static XActionInput Sendout(SimPokemon sendout)
    {
      return new XActionInput(0, 0, 0, sendout.IndexInOwner);
    }
    public static XActionInput Struggle()
    {
      return new XActionInput(0, 0, 0, 0);
    }

    [DataMember(EmitDefaultValue = false)]
    public readonly int Move;

    [DataMember(EmitDefaultValue = false)]
    public readonly byte TargetTeam;

    [DataMember(EmitDefaultValue = false)]
    public readonly byte TargetX;

    [DataMember(EmitDefaultValue = false)]
    public readonly byte SendoutIndex;

    private XActionInput(int move, int targetTeam, int targetX, int sendout)
    {
      Move = move;
      TargetTeam = (byte)targetTeam;
      TargetX = (byte)targetX;
      SendoutIndex = (byte)sendout;
    }
  }
  [DataContract(Namespace = PBOMarks.PBO)]
  public class ActionInput
  {
    [DataMember]
    public readonly XActionInput[] Inputs;

    public ActionInput(int maxX)
    {
      Inputs = new XActionInput[maxX];
    }

    public void UseMove(int x, SimMove move, int targetTeam, int targetX)
    {
      Inputs[x] = XActionInput.UseMove(move, targetTeam, targetX);
    }
    public void UseMove(int x, SimMove move)
    {
      Inputs[x] = XActionInput.UseMove(move);
    }
    public void Switch(int x, SimPokemon sendout)
    {
      Inputs[x] = XActionInput.Sendout(sendout);
    }
    public void Sendout(int x, SimPokemon sendout)
    {
      Inputs[x] = XActionInput.Sendout(sendout);
    }
    public void Struggle(int x)
    {
      Inputs[x] = XActionInput.Struggle();
    }
  }
}
