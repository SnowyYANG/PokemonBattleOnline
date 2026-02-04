using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using PokemonBattleOnline.Network.C2SEs;

namespace PokemonBattleOnline.Network
{
  [JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
  [JsonDerivedType(typeof(ClientInitC2S), "init")]
  [JsonDerivedType(typeof(ChatC2S), "chat")]
  internal interface IC2SE
  {
    void Execute(PboUser user);
  }
}
