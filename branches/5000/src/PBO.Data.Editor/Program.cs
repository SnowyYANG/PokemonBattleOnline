﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.IO.Compression;
using System.IO.Packaging;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace PokemonBattleOnline.Game.DataEditor
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

    static string Desktop
    { get { return string.Format(@"c:\users\{0}\desktop\", Environment.UserName); } }
    static int GetNumberBW(int id)
    {
      switch (id)
      {
        case 650: return 386;
        case 651: return 386;
        case 652: return 386;
        case 653: return 413;
        case 654: return 413;
        case 655: return 492;
        case 656: return 487;
        case 657: return 479;
        case 658: return 479;
        case 659: return 479;
        case 660: return 479;
        case 661: return 479;
        case 662: return 351;
        case 663: return 351;
        case 664: return 351;
        case 665: return 550;
        case 666: return 555;
        case 667: return 648;
        default: return id;
      }
    }
    static int GetNumberBW2(int id)
    {
      switch (id)
      {
        case 685: return 386;
        case 686: return 386;
        case 687: return 386;
        case 688: return 413;
        case 689: return 413;
        case 690: return 492;
        case 691: return 487;
        case 692: return 479;
        case 693: return 479;
        case 694: return 479;
        case 695: return 479;
        case 696: return 479;
        case 697: return 351;
        case 698: return 351;
        case 699: return 351;
        case 700: return 550;
        case 701: return 555;
        case 702: return 648;
        case 703: return 646;
        case 704: return 646;
        case 705: return 647;
        case 706: return 641;
        case 707: return 642;
        case 708: return 645;
        default: return id;
      }
    }
    static void Depress(string input, string output)
    {
      using (var ifs = new FileStream(input, FileMode.Open, FileAccess.Read))
      using (var ds = new DeflateStream(ifs, CompressionMode.Decompress))
      using (var ofs = new FileStream(output, FileMode.Create))
        ds.CopyTo(ofs);
    }
    static void CreatEmptyZip(string path)
    {
      using (var zip = ZipPackage.Open(path, FileMode.Create))
      {
        zip.CreatePart(new Uri("/test.txt", UriKind.Relative), System.Net.Mime.MediaTypeNames.Text.Plain);
        zip.CreatePart(new Uri("/test.csv", UriKind.Relative), System.Net.Mime.MediaTypeNames.Text.Plain);
        zip.CreatePart(new Uri("/test.xml", UriKind.Relative), System.Net.Mime.MediaTypeNames.Text.Xml);
      }
    }
    static void Main(string[] args)
    {
      //using (var pack = new ZipData("..\\res\\rom.zip"))
      //{
      //  RomData.Load(pack, "/rom.xml");
      //  using (var sw = new StreamWriter(Desktop + "flingPower.txt"))
      //    foreach (var i in RomData.Items) sw.WriteLine(i.FlingPower);
      //  System.Diagnostics.Debugger.Break();
      //}

      //CreatEmptyZip(Desktop + "test.zip");
      //Depress(Desktop + "rom.dat", Desktop + "rom.xml");

      var gl = new GameLogs();
      gl.logs.Add("test", new LogText("test"));
      gl.SaveXml(Desktop + "logs.xml");

      Console.ReadKey();
    }
  }
}
