using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.Host;

namespace LightStudio.PokemonBattle.Game.GameEvents
{
  [DataContract(Namespace = Namespaces.LIGHT)]
  public class UseMove : GameEvent
  {
    [DataMember]
    int Pm;
    [DataMember]
    int Move;
    [DataMember(EmitDefaultValue = false)]
    public int PP;

    public UseMove(PokemonProxy pm, MoveType move)
    {
      Pm = pm.Id;
      Move = move.Id;
    }

    protected override void Update()
    {
      var pm = Game.GetPokemon(Pm);
      pm.ChangePosition(pm.Position.X, CoordY.Plate);
      if (Move == Host.Sp.Moves.STRUGGLE) AppendGameLog("Struggle", Pm);
      AppendGameLog("UseMove", Pm, Move);
    }
    public override void Update(SimGame game)
    {
      var pm = game.OnboardPokemons.FirstOrDefault((p) => p.Id == Pm);
      if (pm != null)
        foreach (var m in pm.Moves)
          if (m != null && m.Type.Id == Move) m.PP.Value -= 1 + PP;
    }
  }
}
