using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Game;
using LightStudio.PokemonBattle.Data;


namespace LightStudio.PokemonBattle.Effects.Moves
{
  class SpRangeMove : AttackMoveE
  {
    private readonly CoordY Y;
    private readonly bool Doubled;

    public SpRangeMove(int mId, CoordY y, bool doubled = false)
      : base(mId)
    {
      Y = y;
      Doubled = doubled;
    }
    protected override bool IsYInRange(LightStudio.PokemonBattle.Game.DefContext def)
    {
      return def.Defender.OnboardPokemon.CoordY == Y || base.IsYInRange(def);
    }
    protected override void Calculate(DefContext def)
    {
      base.Calculate(def);
      if (def.Defender.OnboardPokemon.CoordY == Y) def.Damage <<= 1;
    }
  }
  class GustTwister : AttackMoveE
  {
    public GustTwister(int moveId)
      : base(moveId)
    {
    }
    protected override bool IsYInRange(DefContext def)
    {
      return def.Defender.OnboardPokemon.CoordY == CoordY.Air || base.IsYInRange(def);
    }
    protected override void CalculateBasePower(DefContext def)
    {
      if (def.Defender.OnboardPokemon.CoordY == CoordY.Air) def.BasePower = 80;
      else base.CalculateBasePower(def);
    }
  }
  class Thunder : SpRangeMove
  {
    public Thunder(int id)
      : base(id, CoordY.Air)
    {
    }
    public override int GetAccuracyBase(AtkContext atk)
    {
      int r;
      Weather w = atk.Controller.GetAvailableWeather();
      if (w == Weather.HeavyRain) r = 0x65;
      else if (w == Weather.IntenseSunlight) r = 50;
      else r = base.GetAccuracyBase(atk);
      return r;
    }
  }
}
