using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.Host.Sp;
using As = LightStudio.PokemonBattle.Game.Host.Sp.Abilities;
using Is = LightStudio.PokemonBattle.Game.Host.Sp.Items;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Attack
{
  class BeatUp : AttackMoveE
  {
    public BeatUp(int id)
      : base(id)
    {
    }

    protected override void Act(AtkContext atk)
    {
      PokemonProxy aer = atk.Attacker;

      int hits = 0;
      foreach(var pm in aer.Pokemon.Owner.Pokemons)
        if (pm == aer.Pokemon || pm.State == PokemonState.Normal)
        {
          hits++;
          atk.Attachment = pm == aer.Pokemon? aer.OnboardPokemon.Form.BaseAtk : pm.Form.BaseAtk;
          CalculateDamages(atk);
          Implement(atk.Targets);
          if (atk.Target.Defender.Hp == 0 || aer.Hp == 0 || aer.State == PokemonState.FRZ || aer.State == PokemonState.SLP) break;
        }
      atk.Controller.ReportBuilder.Add("Hits", hits);

      PostEffect(atk);
    }
    protected override void CalculateBasePower(DefContext def)
    {
      def.BasePower = def.AtkContext.Attachment / 10 + 5;
    }
    protected override void CalculateEffectRevise(DefContext def)
    {
    }
  }
}
