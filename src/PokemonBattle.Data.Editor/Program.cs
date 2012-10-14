using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Xml;
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

    static void Main(string[] args)
    {
    }
  }
}
