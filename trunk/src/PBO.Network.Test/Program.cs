using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace PokemonBattleOnline.Network.Test
{
  [DataContract(Name = "a")]
  class A
  {
    [DataMember]
    protected int I;

    public A(int i)
    {
      I = i;
    }
    protected A()
    {
    }
  }
  [DataContract(Name = "a")]
  class B : A
  {
    public void Execute()
    {
      Console.Write(I);
    }
  }
  
  class Program
  {
    static void Main(string[] args)
    {
      byte[] buffer;
      using (var s = new System.IO.MemoryStream())
      {
        var a = new A(3);
        var ser = new DataContractJsonSerializer(typeof(object), new Type[] { typeof(A) });
        ser.WriteObject(s, a);
        buffer = s.ToArray();
      }
      using (var s = new System.IO.MemoryStream(buffer, false))
      {
        var der = new DataContractJsonSerializer(typeof(object), new Type[] { typeof(B) });
        var b = (B)der.ReadObject(s);
        b.Execute();
      }
      Console.ReadKey();
    }
  }
}
