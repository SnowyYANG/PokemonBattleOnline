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
    /// <summary>
    /// MoveType.Id
    /// </summary>
    [DataMember]
    int Move;

    public UseMove(MoveProxy m)
    {
      Pm = m.Owner.Id;
      Move = m.Type.Id;
    }

    PokemonOutward pm;
    public override void Update(GameOutward game)
    {
      pm = game.GetPokemon(Pm);
    }
    public override IText GetGameLog()
    {
      IText t = GetGameLog("UseMove");
      t.SetData(pm, DataService.GetMoveType(Move).GetLocalizedName());
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
