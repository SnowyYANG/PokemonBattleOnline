using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LightStudio.PokemonBattle.Data
{
  [DataContract(Namespace = PBOMarks.PBO)]
  public class MoveTypeAdvancedFlags
  {
    [DataMember(EmitDefaultValue = false)]
    public bool IsFist { get; private set; }

    [DataMember(EmitDefaultValue = false)]
    public bool Mirrorable { get; private set; }

    [DataMember(EmitDefaultValue = false)]
    public bool Snatchable { get; private set; }

    [DataMember(EmitDefaultValue = false)]
    public bool MagicCoat { get; private set; }

    [DataMember(EmitDefaultValue = false)]
    public bool Protectable { get; private set; }

    [DataMember(EmitDefaultValue = false)]
    public bool StiffOneTurn { get; private set; }

    [DataMember(EmitDefaultValue = false)]
    public bool PrepareOneTurn { get; private set; }

    [DataMember(EmitDefaultValue = false)]
    public bool NeedTouch { get; private set; }

    [DataMember(EmitDefaultValue = false)]
    public bool IgnoreSubstitute { get; private set; }

    [DataMember(EmitDefaultValue = false)]
    public bool IsHeal { get; private set; }

    [DataMember(EmitDefaultValue = false)]
    public bool IsRemote { get; private set; }

    [DataMember(EmitDefaultValue = false)]
    public bool AvailableEvenFrozen { get; private set; }

    [DataMember(EmitDefaultValue = false)]
    public bool UnavailableWithGravity { get; private set; }

    [DataMember(EmitDefaultValue = false)]
    public bool IsSound { get; private set; }
  }
}
