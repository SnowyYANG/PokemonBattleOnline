using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Game;
using LightStudio.PokemonBattle.Data;


namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Attack
{
  class SLevel : AttackMoveE
  {
    private readonly int Const;
    
    public SLevel(int MoveId, int @const)
      : base(MoveId)
    {
      Const = @const;
    }

    private int Positive(int x)
    {
      return x > 0 ? x : 0;
    }

    protected override void CalculateBasePower(DefContext def)
    {
      var l5 = def.Defender.OnboardPokemon.Lv5D;
      int sst = l5.Atk;
      sst += Positive(l5.Def);
      sst += Positive(l5.SpAtk);
      sst += Positive(l5.SpDef);
      sst += Positive(l5.Speed);
      sst += Positive(def.Defender.OnboardPokemon.AccuracyLv);
      sst += Positive(def.Defender.OnboardPokemon.EvasionLv);
      def.BasePower = sst * 20 + Const;
    }
  }
}
