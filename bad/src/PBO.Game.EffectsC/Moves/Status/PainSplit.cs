using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Game.Host.Effects.Moves.Status
{
  [DataContract(Namespace = PBOMarks.JSON)]
  class PainSplitEvent : GameEvent
  {
    [DataMember]
    int A;
    [DataMember]
    int D;

    public PainSplitEvent(AtkContext atk)
    {
      A = atk.Attacker.Id;
      D = atk.Target.Defender.Id;
    }

    int hp;
    protected override void Update()
    {
      var a = GetPokemon(A);
      var d = GetPokemon(D);
      hp = (a.Hp.Value + d.Hp.Value) >> 1;
      a.Hp.Value = hp > a.Hp.Origin ? a.Hp.Origin : hp;
      d.Hp.Value = hp > d.Hp.Origin ? d.Hp.Origin : hp;
      AppendGameLog("PainSplit", A, D);
    }
    public override void Update(SimGame game)
    {
      var pm = GetPokemon(game, A);
      if (pm != null) pm.SetHp(hp);
      pm = GetPokemon(game, D);
      if (pm != null) pm.SetHp(hp);
    }
  }
  class PainSplit : StatusMoveE
  {
    static PainSplit()
    {
      EffectsService.Register<PainSplitEvent>();
    }

    public PainSplit(int id)
      : base(id)
    {
    }

    protected override void Act(AtkContext atk)
    {
      int hp = (atk.Attacker.Hp + atk.Target.Defender.Hp) >> 1;
      atk.Attacker.Pokemon.SetHp(hp);
      atk.Target.Defender.Pokemon.SetHp(hp);
      atk.Controller.ReportBuilder.Add(new PainSplitEvent(atk));
    }
  }
}
