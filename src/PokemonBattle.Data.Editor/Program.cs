using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Xml;
using LightStudio.Tactic.DataModels;
using LightStudio.Tactic.Serialization;

namespace LightStudio.PokemonBattle.Data.Editor
{
  class Program
  {
    static byte ToByte(char hex)
    {
      return (byte)(hex >= 'A' ? hex - 'A' + 10 : hex - '0');
    }
    static byte ToByte(string hex)
    {
      return (byte)(ToByte(hex[0]) * 16 + ToByte(hex[1]));
    }
    static sbyte ToSByte(string hex)
    {
      int i = ToByte(hex);
      if (i > 127) i -= 256;
      return (sbyte)i;
    }

    static void Main(string[] args)
    {
    }
  }
}
