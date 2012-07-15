using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Interactive.GameEvents
{
  [DataContract(Namespace = Namespaces.DEFAULT)]
  class UseMove : GameEvent
  {
    [DataMember]
    int Pm;
    [DataMember]
    int Move;

    public UseMove(PokemonProxy pm, MoveType move)
    {
      Pm = pm.Id;
      Move = move.Id;
    }

    PokemonOutward pm;
    public override void Update(GameOutward game)
    {
      pm = game.GetPokemon(Pm);
      pm.ChangePosition(pm.Position.X, CoordY.Plate);
    }
    public override IText GetGameLog()
    {
      IText t = GetGameLog("UseMove");
      t.SetData(pm, DataService.GetMove(Move).GetLocalizedName());
      return t;
    }
    public override void Update(SimGame game)
    {
      Pokemon pm = game.Team.Pokemons.ValueOrDefault(Pm);
      if (pm != null)
        foreach (Move m in pm.Moves)
          if (m != null && m.Type.Id == Move) m.PP.Value--;
    }
  }
}
