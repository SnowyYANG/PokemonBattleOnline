using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Status
{
  class Assist : StatusMoveE
  {
    #region BLOCK
    private readonly int[] BLOCK = 
    {
      166,
      274,
      118,
      165,
      271,
      415,
      214,
      382,
      448,
      68,
      243,
      194,
      182,
      197,
      203,
      364,
      264,
      266,
      476,
      270,
      383,
      119,
      289,
      525,
      509,
      144
    };
    #endregion

    public Assist(int id)
      : base(id)
    {
    }

    public override void Execute(AtkContext atk)
    {
      var aer = atk.Attacker;
      var moves = new List<MoveType>();
      foreach (var pm in aer.Tile.Field.Pokemons)
        if (pm != aer && pm.Pokemon.Owner == aer.Pokemon.Owner)
          foreach (var m in pm.Moves)
            if (!BLOCK.Contains(m.Type.Id)) moves.Add(m.Type);
      for (int i = aer.Controller.GameSettings.Mode.OnboardPokemonsPerPlayer(); i < aer.Pokemon.Owner.Pokemons.Count(); ++i)
        foreach (var m in aer.Pokemon.Owner.GetPokemon(i).Moves)
          if (!BLOCK.Contains(m.Type.Id)) moves.Add(m.Type);
      if (moves.Count == 0) CallMove(atk, moves[aer.Controller.GetRandomInt(0, moves.Count - 1)]);
      else FailAll(atk);
    }
  }
}
