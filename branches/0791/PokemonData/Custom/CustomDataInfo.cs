using System;
using System.Collections.Generic;
using System.Text;

namespace PokemonBattle.PokemonData.Custom
{
    [Serializable()]
    public class CustomDataInfo
    {

        public string DataName
        { get; set; }

        public string DataHash
        { get; set; }

        public CustomDataInfo()
        {
            DataName = string.Empty;
            DataHash = string.Empty;
        }

        public override string ToString()
        {
            return DataName;
        }
    }
}
