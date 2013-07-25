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
    int PP;
    [DataMember(EmitDefaultValue = false)]
    string Log;
    [DataMember(EmitDefaultValue = false)]
    public int Arg2;
    [DataMember(EmitDefaultValue = false)]
    public bool Item;

    internal PPChange(string log, Host.MoveProxy move)
    {
      Log = log;
      Pm = move.Owner.Id;
      Move = move.Type.Id;
      PP = move.PP;
    }

    protected override void Update()
    {
      AppendGameLog(Log, Pm, Move, Arg2);
    }
    public override void Update(SimGame game)
    {
      var pm = game.OnboardPokemons.FirstOrDefault((p) => p.Id == Pm);
      if (pm != null)
      {
        foreach (var m in pm.Moves)
          if (m != null && m.Type.Id == Move)
          {
            m.PP.Value = PP;
            break;
          }
        if (Item) pm.Pokemon.Item = null;
      }
    }
  }
}
