using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Game
{
  [DataContract(Namespace = PBOMarks.PBO)]
  public class MoveFlags
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
    public bool Protectable
#if EDITING
;
#else
    { get; private set; }
#endif

    [DataMember(EmitDefaultValue = false)]
    public bool StiffOneTurn
#if EDITING
;
#else
    { get; private set; }
#endif

    [DataMember(EmitDefaultValue = false)]
    public bool PrepareOneTurn
#if EDITING
;
#else
    { get; private set; }
#endif

    [DataMember(EmitDefaultValue = false)]
    public bool NeedTouch
#if EDITING
;
#else
    { get; private set; }
#endif

    [DataMember(EmitDefaultValue = false)]
    public bool IgnoreSubstitute
#if EDITING
;
#else
    { get; private set; }
#endif

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
