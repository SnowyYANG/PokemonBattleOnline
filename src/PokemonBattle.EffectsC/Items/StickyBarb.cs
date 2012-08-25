using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LightStudio.PokemonBattle.Game.Host.Effects.Items
{
  /// <summary>
  /// transform the sticky barb from a to b silently
  /// </summary>
  [DataContract(Namespace = Namespaces.LIGHT)]
  class StickyBarbEvent : GameEvent
  {
    public StickyBarbEvent(PokemonProxy defender, PokemonProxy attacker)
    {
      D = defender.Id;
      A = attacker.Id;
    }
    
    [DataMember]
    int A; //get stickybarb
    [DataMember]
    int D; //lost stickybarb
    
    public override void Update(SimGame game)
    {
      var a = GetPokemon(game, D);
      if (a != null) a.Item = null;
      else
      {
        var b = GetPokemon(game, A);
        b.Item = Data.DataService.GetItem(65);
      }
    }
  }
  class StickyBarb : ItemE
  {
    static StickyBarb()
    {
      EffectsService.Register<StickyBarbEvent>();
    }
    public StickyBarb(int id)
      : base(id)
    {
    }
    public override void Attacked(DefContext def)
    {
      var der = def.Defender;
      var aer = def.AtkContext.Attacker;
      if (aer.Pokemon.Item == null && def.AtkContext.Move.AdvancedFlags.NeedTouch && der.Controller.RandomHappen(10))
      {
        der.Pokemon.Item = null;
        aer.ChangeItem(65);
        der.Controller.ReportBuilder.Add(new StickyBarbEvent(der, aer));
      }
    }
  }
}
