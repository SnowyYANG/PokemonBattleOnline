using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Game.Host;

namespace LightStudio.PokemonBattle.Game.GameEvents
{
  [DataContract(Namespace = PBOMarks.PBO)]
  internal class PainSplit : GameEvent
  {
    [DataMember]
    int A;
    [DataMember]
    int D;

    public PainSplit(AtkContext atk)
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
}
