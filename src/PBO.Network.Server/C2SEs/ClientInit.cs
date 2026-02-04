using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PokemonBattleOnline.Network.C2SEs
{
    [DataContract(Name ="init", Namespace = PBOMarks.JSON)]
    internal class ClientInitC2S : Commands.ClientInitC2S, IC2SE
    {
        public void Execute(PboUser su)
        {
            su.Server.AddUser(su, name, room, seat);
        }
    }
}
