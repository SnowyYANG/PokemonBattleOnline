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
        Console.WriteLine(PBOClient.Current.Controller.User.Name);
        foreach (var u in PBOClient.Current.Controller.Users) Console.WriteLine("{0} {1}", u.Id, u.Name);
      }
      Console.WriteLine("---");
    }
    
    static void Main(string[] args)
    {
      PBOServer.NewServer();
      PBOClient.CurrentChanged += () =>
        {
          ((ObservableList<User>)PBOClient.Current.Controller.Users).CollectionChanged += (sender, e) => Print();
          ClientController.PublicChat += (s, u) => Console.WriteLine(u.Name + ": " + s);
        };
      PBOClient.Login("127.0.0.1", "t1", 408);

      Console.ReadKey();
      
      PBOClient.Current.Controller.ChatPublic("hello");

      Console.ReadKey();

      PBOClient.Dispose();

      PBOClient.Login("127.0.0.1", "t2", 408);

      Console.ReadKey();

      PBOClient.Dispose();

      Console.ReadKey();
    }
  }
}
