using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using PokemonBattleOnline.Game;
using PokemonBattleOnline.Network;

namespace PokemonBattleOnline.PBO.Server
{
  class Program
  {
    static int Main(string[] args)
    {
      using (var pack = new ZipData("..\\res\\rom.zip"))
      {
        RomData.Load(pack, "/rom.xml");
        LearnList.Load(pack, "/learnset");
      }
      PBOServer.NewServer(PBOMarks.DEFAULT_PORT);
      var server = PBOServer.Current;
      server.Start();
      Thread t = new Thread(TaskbarIcon.Init);
      t.Start();

    LOOP:
      var line = Console.ReadLine();
      if (line.Equals("quit", StringComparison.CurrentCultureIgnoreCase))
      {
        TaskbarIcon.Close();
        return 0;
      }
      goto LOOP;
    }
  }
}
