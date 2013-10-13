using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace PokemonBattleOnline.Network.Test
{
  class Program
  {
    private static void Print()
    {
      Console.WriteLine();
      Console.WriteLine("Server");
      foreach (var u in PBOServer.Current.State.Users) Console.WriteLine("{0} {1}", u.Id, u.Name);
      if (PBOClient.Current == null) Console.WriteLine("null");
      else
      {
        Console.WriteLine(PBOClient.Current.State.User.Name);
        foreach (var u in PBOClient.Current.State.Users) Console.WriteLine("{0} {1}", u.Id, u.Name);
      }
      Console.WriteLine("---");
    }
    
    static void Main(string[] args)
    {
      PBOServer.NewServer();
      PBOServer.Current.State.Users.CollectionChanged += (sender, e) => Print();
      PBOClient.CurrentChanged += () =>
        {
          PBOClient.Current.State.Users.CollectionChanged += delegate { };
        };
      PBOClient.Login("127.0.0.1", "t1", 408);

      Console.ReadKey();

      PBOClient.Dispose();

      PBOClient.Login("127.0.0.1", "t2", 408);

      Console.ReadKey();

      PBOClient.Dispose();

      Console.ReadKey();
    }
  }
}
