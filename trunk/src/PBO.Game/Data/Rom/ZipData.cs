using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Packaging;

namespace PokemonBattleOnline.Game
{
  public class ZipData : IDisposable
  {
    private readonly Package Pack;
    
    public ZipData(string pack)
    {
      Pack = ZipPackage.Open(new FileStream(pack, FileMode.Open, FileAccess.Read, FileShare.Read));
    }

    public void Dispose()
    {
      ((IDisposable)Pack).Dispose();
    }
  }
}
