using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game.Host;

namespace LightStudio.PokemonBattle.Game.GameEvents
{
  [DataContract(Namespace = PBOMarks.PBO)]
  internal class ReHurtBerry : GameEvent
  {
    [DataMember]
    int A;
    
    [DataMember(EmitDefaultValue = false)]
    int Hp;

    [DataMember]
    int D;

    [DataMember(EmitDefaultValue = false)]
    int Item;
    
    public ReHurtBerry(PokemonProxy attacker, PokemonProxy defender)
    {
      A = attacker.Id;
      Hp = attacker.Hp;
      D = defender.Id;
      Item = defender.Pokemon.Item.Id - 191;
    }
    protected override void Update()
    {
      var pm = GetPokemon(A);
      var oldHp = pm.Hp.Value;
      pm.Hp.Value = Hp;
      AppendGameLog("ReHurtItem", A, D, Item + 191);
      AppendGameLog("Hp", Hp - oldHp);
    }
    public override void Update(SimGame game)
    {
      var pm = GetPokemon(game, A);
      if (pm != null) ((PairValue)pm.Hp).Value = Hp;
      pm = GetPokemon(game, D);
      if (pm != null) pm.Item = null;
    }
  }
}
