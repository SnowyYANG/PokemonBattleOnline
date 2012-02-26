using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LightStudio.PokemonBattle.Data
{
  [DataContract(Namespace = Namespaces.DEFAULT)]
  internal class DataConfiguration
  {
    [DataMember]
    public string IconPath
    { get; set; }

    [DataMember]
    public string FrontPath
    { get; set; }

    [DataMember]
    public string FemaleFrontPath
    { get; set; }

    [DataMember]
    public string BackPath
    { get; set; }

    [DataMember]
    public string FemaleBackPath
    { get; set; }

  }
}
