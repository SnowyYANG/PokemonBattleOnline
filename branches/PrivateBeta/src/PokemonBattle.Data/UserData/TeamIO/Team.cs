using System;
using System.Xml.Serialization;

namespace LightStudio.PokemonBattle.Data.PokemonOnline
{
    /// <summary>
    /// PO Team对象
    /// </summary>
    [Serializable]
    public class Team
    {
        [XmlAttribute("gen")]
        public string Gen { get; set; }

        [XmlAttribute("subgen")]
        public string Subgen { get; set; }

        [XmlAttribute("defaultTier")]
        public string DefaulTtier { get; set; }

        [XmlAttribute("version")]
        public string Version { get; set; }

        [XmlElement("Pokemon")]
        public Pokemon[] Pokemons { get; set; }

        #region po2pbo/pbo2po

        public PokemonBT ToPokemonBT()
        {
            var bt = new PokemonBT();

            for (int i = 0; i < 6 && i < this.Pokemons.Length; i++)
            {
                var pm = Pokemons[i].ToPokemonData();
                if (pm != null) bt.Add(pm);
            }

            return bt;
        }
        
        public void FromPokemonBT(PokemonBT bt)
        {
            this.Pokemons = new Pokemon[bt.Count];
            for (int i = 0; i < bt.Count; i++)
            {
                this.Pokemons[i] = Pokemon.FromPokemonData(bt[i]);
            }
        }

        #endregion
    }
}
