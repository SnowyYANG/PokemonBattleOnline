using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LightStudio.PokemonBattle.Data
{

    [DataContract(Namespace = Namespaces.LIGHT)]
    internal class UserPokemonDataInfo
    {
        [DataMember]
        public CollectionInfo Teams
        { get; private set; }

        [DataMember]
        public CollectionInfo Boxes
        { get; private set; }

        [DataMember]
        public RecyclerInfo Recycler
        { get; private set; }

        [DataMember]
        public int SaveInterval
        { get; private set; }

        public UserPokemonDataInfo(UserPokemonData data)
        {
            this.Teams = CollectionInfo.FromCollection(data.Teams);
            this.Boxes = CollectionInfo.FromCollection(data.Boxes);
            this.Recycler = RecyclerInfo.FromRecycler(data.Recycler);
            this.SaveInterval = data.SaveInterval;
        }

    }
}
