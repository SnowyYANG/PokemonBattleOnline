using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.Host;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Abilities
{
  class Forewarn : AbilityE
  {
    private static int GetPower(MoveType move)
    {
      int r = move.Power;
      if (r == 1)
      {
        if (move.Class == MoveInnerClass.OHKO) r = 160;
        else if (move.Id == 382) r = 0; //先取
        else if (move.Id == 68 || move.Id == 243 || move.Id == 368) r = 120;
      }
      return r;
    }

    public Forewarn(int id)
      : base(id)
    {
    }

    public override void Attach(PokemonProxy pm)
    {
      List<KeyValuePair<PokemonProxy, MoveType>> moves = new List<KeyValuePair<PokemonProxy, MoveType>>();
      {
        int maxPower = 0;
        foreach (PokemonProxy p in pm.Controller.Board[1 - pm.Pokemon.TeamId].GetPokemons(pm.OnboardPokemon.X - 1, pm.OnboardPokemon.X + 1))
          foreach (MoveProxy m in p.Moves)
          {
            int power = GetPower(m.Type);
            if (power > maxPower)
            {
              moves.Clear();
              maxPower = power;
            }
            if (power == maxPower) moves.Add(new KeyValuePair<PokemonProxy, MoveType>(p, m.Type));
          }
      }
      if (moves.Count != 0)
      {
        KeyValuePair<PokemonProxy, MoveType> pair = moves[pm.Controller.GetRandomInt(0, moves.Count - 1)];
        Raise(pm);
        pm.Controller.ReportBuilder.Add("ReadMove", pair.Key, pair.Value);
      }
    }
  }
}
