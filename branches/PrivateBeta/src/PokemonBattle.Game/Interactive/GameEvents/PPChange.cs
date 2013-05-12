using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LightStudio.PokemonBattle.Game.GameEvents
{
  [DataContract(Namespace = Namespaces.PBO)]
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
    [DataMember(EmitDefaultValue = false)]
    public bool Item;

    public PPChange(string log, Host.MoveProxy move, int formerPP)
    {
      Log = log;
      Pm = move.Owner.Id;
      Move = move.Type.Id;
      Change = move.PP - formerPP;
    }

    protected override void Update()
    {
      AppendGameLog(Log, Pm, Move, Math.Abs(Change));
    }
    public override void Update(SimGame game)
    {
      var pm = game.OnboardPokemons.FirstOrDefault((p) => p.Id == Pm);
      if (pm != null)
      {
        foreach (var m in pm.Moves)
          if (m.Type.Id == Move)
          {
            m.PP.Value += Change;
            break;
          }
        if (Item) pm.Pokemon.Item = null;
      }
    }
  }
}
