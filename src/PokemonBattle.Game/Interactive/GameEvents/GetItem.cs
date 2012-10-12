using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using LightStudio.PokemonBattle.Game.Host;

namespace LightStudio.PokemonBattle.Game.GameEvents
{
  [DataContract(Namespace = Namespaces.PBO)]
  public class GetItem : GameEvent
  {
    [DataMember]
    int Pm;
    [DataMember]
    int Item;
    [DataMember(EmitDefaultValue = false)]
    string Key;
    [DataMember(EmitDefaultValue = false)]
    int Loser;

    public GetItem(PokemonProxy pm, string key, PokemonProxy itemLoser)
    {
      Pm = pm.Id;
      Item = pm.Pokemon.Item.Id;
      Key = key == "GetItem" ? null : key;
      if (itemLoser != null) Loser = itemLoser.Id;
    }
    protected override void Update()
    {
      if (Key != null) AppendGameLog(Key ?? "GetItem", Pm, Item, Loser);
    }
    public override void Update(SimGame game)
    {
      var pm = GetPokemon(game, Pm);
      if (pm != null) pm.Item = Data.GameDataService.GetItem(Item);
      if (Loser != 0)
      {
        pm = GetPokemon(game, Loser);
        if (pm != null) pm.Item = null;
      }
    }
  }
}
