using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LightStudio.PokemonBattle.Data
{
  [DataContract(Namespace = Namespaces.DEFAULT)]
  public class MoveTypeAdvancedFlags
  {
    [DataMember]
    public bool IsFist { get; private set; }

    [DataMember]
    public bool Mirrorable { get; private set; }

    [DataMember]
    public bool Snatchable { get; private set; }

    [DataMember]
    public bool MagicCoat { get; private set; }

    [DataMember]
    public bool Protectable { get; private set; }

    [DataMember]
    public bool StiffOneTurn { get; private set; }

    [DataMember]
    public bool PrepareOneTurn { get; private set; }

    [DataMember]
    public bool NeedTouch { get; private set; }

    [DataMember]
    public bool AvailableOnSubstitute { get; private set; }

    [DataMember]
    public bool IsHeal { get; private set; }

    [DataMember]
    public bool IsRemote { get; private set; }

    [DataMember]
    public bool AvailableEvenFrozen { get; private set; }

    [DataMember]
    public bool UnavailableWithGravity { get; private set; }

    [DataMember]
    public bool IsSound { get; private set; }
  }
}
