using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Status
{
  [DataContract(Namespace = Namespaces.PBO)]
  class RestGameEvent : GameEvent
  {
    [DataMember]
    int Pm;

    public RestGameEvent(PokemonProxy pm)
    {
      Pm = pm.Id;
    }

    protected override void Update()
    {
      var pm = GetPokemon(Pm);
      pm.Hp.Value = pm.Hp.Origin;
      pm.State = PokemonState.SLP;
      AppendGameLog("Rest", Pm);
    }
    public override void Update(SimGame game)
    {
      var pm = GetPokemon(game, Pm);
      if (pm != null)
      {
        pm.SetHp(pm.Hp.Origin);
        pm.ClientChangePokemonState(PokemonState.SLP);
      }
    }
  }
  class Rest : StatusMoveE
  {
    static Rest()
    {
      EffectsService.Register<RestGameEvent>();
    }

    public Rest(int id)
      : base(id)
    {
    }

    public override void Execute(AtkContext atk)
    {
      var pm = atk.Attacker;
      if (pm.Hp == pm.Pokemon.Hp.Origin) FailAll(atk, "FullHp", pm.Id);
      else if (pm.Ability.CanAddState(pm, pm, AttachedState.SLP, true)) base.Execute(atk);
    }
    protected override void Act(AtkContext atk)
    {
      var pm = atk.Attacker;
      if (pm.Hp == pm.Pokemon.Hp.Origin) FailAll(atk); //for snatch...
      else if (pm.Ability.CanAddState(pm, pm, AttachedState.SLP, true))
      {
        pm.Controller.ReportBuilder.Add(new RestGameEvent(pm));
        pm.Pokemon.SetHp(pm.Pokemon.Hp.Origin);
        pm.Pokemon.ClientChangePokemonState(PokemonState.SLP);
        pm.OnboardPokemon.SetCondition("SLP", 3);
        pm.Tile.Field.SetCondition("Rest" + pm.Id);
        pm.Item.StateAdded(pm, pm, AttachedState.SLP);
      }
    }
  }
}
