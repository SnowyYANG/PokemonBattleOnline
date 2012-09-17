using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LightStudio.PokemonBattle.Data;
using LightStudio.PokemonBattle.Game;

namespace LightStudio.PokemonBattle.Test
{
  static class AIController
  {
    public static void WriteBattleReport(string text)
    {
      //TODO: write text to files
      Console.Write(text);
    }
    
    private static ITestClient p1;
    private static ITestClient p2;
    public static void TestBegin(ITestClient p1, ITestClient p2)
    {
      //TODO: more
      AIController.p1 = p1;
      AIController.p2 = p2;
    }
    
    //TODO: remove tempData, it is just a sample
    private static readonly UserData tempData = new UserData();
    public static PokemonCustomInfo[] GetP1Team()
    {
      //TODO: remove the following line and write something else
      return tempData.PokemonData.Teams.Folders.First().Pokemons.ToArray();
    }
    public static PokemonCustomInfo[] GetP2Team()
    {
      //TODO: remove the following line and write something else
      return tempData.PokemonData.Teams.Folders.Last().Pokemons.ToArray();
    }
    
    public static void P1_RequireInput(InputRequest input)
    {
      //TODO: use something like the following
      //p1.Pokemon(1); p1.Pokemon(2).... p1.Pokemon(5);
      //p1.Move(0); p1.Move(1)... p1.Move(3)
      //p1.Struggle()
    }
    public static void P2_RequireInput(InputRequest input)
    {
      //TODO: use something like the following
      //p2.Pokemon(1); p2.Pokemon(2).... p2.Pokemon(5)
      //p2.Move(0); p2.Move(1)... p2.Move(3)
      //p2.Struggle()
    }

    public static void TestEnd()
    {
      //do NOT remove the following 2 lines
      p1.Dispose();
      p2.Dispose();
      
      //TODO: more
    }
  }
}
