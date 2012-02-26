using System;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.ComponentModel;

namespace LightStudio.PokemonBattle.Data
{
    [ContractClass(typeof(IPokemonRecyclerContract))]
    public interface IPokemonRecycler : IPokemonFolder
    {
        void ChangeSize(int newSize);
    }
}
