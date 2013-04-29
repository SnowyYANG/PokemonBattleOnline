using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Is = PokemonBattleOnline.Game.Host.Sp.Items;

namespace PokemonBattleOnline.Game.Host.Effects.Items
{
  [DataContract(Namespace = PBOMarks.JSON)]
  class MentalHerbEvent : GameEvent
  {
    [DataMember(EmitDefaultValue = false)]
    int Pm;
    [DataMember(EmitDefaultValue = false)]
    bool Attract;
    [DataMember(EmitDefaultValue = false)]
    bool Encore;
    [DataMember(EmitDefaultValue = false)]
    bool Taunt;
    [DataMember(EmitDefaultValue = false)]
    bool Torment;
    [DataMember(EmitDefaultValue = false)]
    bool Disable;

    public MentalHerbEvent(PokemonProxy pm, bool i, bool e, bool ta, bool to, bool d)
    {
      Pm = pm.Id;
      Attract = i;
      Encore = e;
      Taunt = ta;
      Torment = to;
      Disable = d;
    }

    protected override void Update()
    {
      if (Attract) AppendGameLog("ItemDeAttract", Pm, 8);
      if (Encore) AppendGameLog("DeEncore", Pm);
      if (Taunt) AppendGameLog("DeTaunt", Pm);
      if (Torment) AppendGameLog("DeTorment", Pm);
      if (Disable) AppendGameLog("DeDisable", Pm);
    }
    public override void Update(SimGame game)
    {
      var pm = GetPokemon(game, Pm);
      if (pm != null) pm.Item = null;
    }
  }
  class MentalHerb : ItemE
  {
    static MentalHerb()
    {
      EffectsService.Register<MentalHerbEvent>();
    }

    public MentalHerb(int id)
      : base(id)
    {
    }

    private bool Act(OnboardPokemon pm, string condition)
    {
      return pm.RemoveCondition(condition);
    }
    private void Act(PokemonProxy pm)
    {
      var op = pm.OnboardPokemon;
      bool i = Act(op, "Attract");
      bool e = Act(op, "Encore");
      bool ta = Act(op, "Taunt");
      bool to = Act(op, "Torment");
      bool d = Act(op, "Disable");
      if (i || e || ta || to || d)
      {
        pm.ConsumeItem();
        pm.Controller.ReportBuilder.Add(new MentalHerbEvent(pm, i, e, ta, to, d));
      }
    }

    public override void Attach(PokemonProxy pm)
    {
      Act(pm);
    }
    public override void StateAdded(PokemonProxy pm, PokemonProxy by, Data.AttachedState state)
    {
      Act(pm);
    }
  }
}
