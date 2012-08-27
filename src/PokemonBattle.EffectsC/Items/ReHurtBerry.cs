using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Items
{
  [DataContract(Namespace = Namespaces.LIGHT)]
  class ReHurtBerryEvent : GameEvent
  {
    [DataMember]
    int A;
    
    [DataMember(EmitDefaultValue = false)]
    int Hp;

    [DataMember]
    int D;

    [DataMember(EmitDefaultValue = false)]
    int Item;
    
    public ReHurtBerryEvent(PokemonProxy attacker, PokemonProxy defender)
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
  class ReHurtBerry : ItemE
  {
    static ReHurtBerry()
    {
      EffectsService.Register<ReHurtBerryEvent>();
    }
    
    private readonly MoveCategory Category;

    public ReHurtBerry(int id, MoveCategory category)
      : base(id)
    {
      Category = category;
    }

    public override void Attacked(DefContext def)
    {
      var aer = def.AtkContext.Attacker;
      if (def.AtkContext.Move.Category == Category && aer.CanEffectHurt)
      {
        int hp = aer.Pokemon.Hp.Origin >> 3;
        if (hp == 0) hp = 1;
        aer.Pokemon.SetHp(aer.Hp - hp);
        aer.Controller.ReportBuilder.Add(new ReHurtBerryEvent(aer, def.Defender));
      }
    }
  }
}
