using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LightStudio.PokemonBattle.Game.GameEvents
{
  [DataContract(Namespace = Namespaces.LIGHT)]
  public class PPChange : GameEvent
  {
    [DataMember(EmitDefaultValue = false)]
    int Pm;
    [DataMember(EmitDefaultValue = false)]
    int Move;
    [DataMember(EmitDefaultValue = false)]
    int Change;
    [DataMember(EmitDefaultValue = false)]
    string Log;

    public PPChange(string log, Host.MoveProxy move, int formerPP)
    {
      Log = log;
      Pm = move.Owner.Id;
      Move = move.Type.Id;
      Change = move.PP - formerPP;
    }

    protected override void Update()
    {
      AppendGameLog(Log, Pm, Move, Change);
    }
    public override void Update(SimGame game)
    {
      var pm = GetPokemon(game, Pm);
      if (pm != null)
      {
        foreach (var m in pm.Moves)
          if (m.Type.Id == Move)
          {
            m.PP.Value += Change;
            break;
          }
      }
    }
  }
}
