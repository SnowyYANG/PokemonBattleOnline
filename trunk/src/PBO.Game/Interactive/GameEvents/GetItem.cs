using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using PokemonBattleOnline.Game.Host;

namespace PokemonBattleOnline.Game.GameEvents
{
  [DataContract(Namespace = PBOMarks.PBO)]
  public class GetItem : GameEvent
  {
    [DataMember]
    int Pm;
    [DataMember]
    int Item;
    [DataMember(EmitDefaultValue = false)]
    string Key;
    [DataMember(EmitDefaultValue = false)]
    int Loster;

    internal GetItem(PokemonProxy pm, string key, PokemonProxy formerOwner)
    {
      Pm = pm.Id;
      Item = pm.Pokemon.Item.Id;
      Key = key;
      if (formerOwner != null) Loster = formerOwner.Id;
    }
    protected override void Update()
    {
      if (Key != null) AppendGameLog(Key, Pm, Item, Loster);
    }
    public override void Update(SimGame game)
    {
      var pm = GetPokemon(game, Pm);
      if (pm != null) pm.Item = Data.GameDataService.GetItem(Item);
      if (Loster != 0)
      {
        pm = GetPokemon(game, Loster);
        if (pm != null) pm.Item = null;
      }
    }
  }
}
