using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Game;
using LightStudio.PokemonBattle.Data;


namespace LightStudio.PokemonBattle.Effects.Moves
{
  class Punishment:AttackMoveE 
  {
    public Punishment(int MoveId)
      : base(MoveId)
    { }

    protected override void CalculateBasePower(DefContext def)
    {
      int sst = def.Defender.OnboardPokemon.Lv5D.Atk;
      sst += def.Defender.OnboardPokemon.Lv5D.Def;
      sst += def.Defender.OnboardPokemon.Lv5D.SpAtk;
      sst += def.Defender.OnboardPokemon.Lv5D.SpDef;
      sst += def.Defender.OnboardPokemon.Lv5D.Speed;
      sst += def.Defender.OnboardPokemon.AccuracyLv;
      sst += def.Defender.OnboardPokemon.EvasionLv;
      sst = sst * 20 + 60;
      if (sst > 120)
        def.BasePower = 120;
      else
        def.BasePower = sst;
    }
  }

  class StoredPower : AttackMoveE
  {
    public StoredPower(int MoveId)
      : base(MoveId)
    { }

    protected override void CalculateBasePower(DefContext def)
    {
      int sst = def.AtkContext.Attacker.OnboardPokemon.Lv5D.Atk;
      sst += def.AtkContext.Attacker.OnboardPokemon.Lv5D.Def;
      sst += def.AtkContext.Attacker.OnboardPokemon.Lv5D.SpAtk;
      sst += def.AtkContext.Attacker.OnboardPokemon.Lv5D.SpDef;
      sst += def.AtkContext.Attacker.OnboardPokemon.Lv5D.Speed;
      sst += def.AtkContext.Attacker.OnboardPokemon.AccuracyLv;
      sst += def.AtkContext.Attacker.OnboardPokemon.EvasionLv;
      def.BasePower = sst * 20;
    }
  }
}
