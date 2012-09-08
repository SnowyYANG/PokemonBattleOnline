using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Moves.Status
{
  [DataContract(Namespace = Namespaces.LIGHT)]
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
      pm.State = PokemonState.Sleeping;
      AppendGameLog("Rest", Pm);
    }
    public override void Update(SimGame game)
    {
      var pm = GetPokemon(game, Pm);
      if (pm != null)
      {
        pm.SetHp(pm.Hp.Origin);
        pm.ClientChangePokemonStateWithNotify(PokemonState.Sleeping);
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

    protected override bool NotFail(AtkContext atk)
    {
      return atk.Attacker.Ability.CanAddState(atk.Attacker, atk.Attacker, AttachedState.Sleep, false);
    }
    protected override void Act(AtkContext atk)
    {
      var pm = atk.Attacker;
      if (pm.Hp == pm.Pokemon.Hp.Origin) pm.AddReportPm("FullHp");
      else
      {
        pm.Controller.ReportBuilder.Add(new RestGameEvent(pm));
        pm.Pokemon.SetHp(pm.Pokemon.Hp.Origin);
        pm.Pokemon.ClientChangePokemonStateWithNotify(PokemonState.Sleeping);
        pm.OnboardPokemon.SetCondition("Sleeping", 3);
        pm.Item.StateAdded(pm, pm, AttachedState.Sleep);
      }
    }
  }
}
