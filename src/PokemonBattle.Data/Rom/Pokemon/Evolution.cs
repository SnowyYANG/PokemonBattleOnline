﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace LightStudio.PokemonBattle.Data
{
  [DataContract(Namespace = Namespaces.PBO)]
  internal sealed class Evolution
  {
#if DEBUG
    public
#else
    private
#endif
      Evolution(int from, int to)
    {
      _from = (short)from;
      _to = (short)to;
    }
    
    [DataMember]
    private readonly short _from;  
    public int From
    { get { return _from; } }
    
    [DataMember]
    private readonly short _to;  
    public int To
    { get { return _to; } }
    
    public bool NeedLvUp
    { get { return false; } }
  }
}