using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LightStudio.PokemonBattle.PBO.Editor
{
    [Flags]
    public enum DragDropActions
    {
        None = 0,
        MoveTo = 1,
        CopyTo = 2,
        SwapWith = 4,
        Replace = 8,
        All = 15
    }
}
