using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace PokemonBattleOnline
{
  public class ZipData : IDisposable
  {
    private readonly ZipArchive Pack;
    
    public ZipData(string pack)
    {
      Pack = new ZipArchive(new FileStream(pack, FileMode.Open, FileAccess.Read, FileShare.Read), ZipArchiveMode.Read);
    }

    public Stream GetStream(string path)
    {
      if (!string.IsNullOrEmpty(path) && path[0] != '/') path = "/" + path;
      var entry = Pack.GetEntry(path.TrimStart('/'));
      return entry?.Open();
    }

    public void Dispose()
    {
      ((IDisposable)Pack).Dispose();
    }
  }
}
