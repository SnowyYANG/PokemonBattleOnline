using System;
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
    static bool ChangeP(string name, int former, int value)
    {
      if (GameString.Move(name)._power == former)
      {
        GameString.Move(name)._power = value;
        return true;
      }
      return false;
    }
    static bool ChangeA(string name, int former, int value)
    {
      if (GameString.Move(name)._accuracy == former)
      {
        GameString.Move(name)._accuracy = value;
        return true;
      }
      return false;
    }
    static bool SetForms(int number, int forms)
    {
      forms++;
      var pm = RomData.GetPokemon(number);
      if (pm.formData.Length == forms)
      {
        pm.forms = new PokemonForm[forms];
        for (int i = 0; i < forms; ++i) pm.forms[i] = new PokemonForm() { _data = i, _index = i };
        return true;
      }
      System.Diagnostics.Debugger.Break();
      return false;
    }
    static bool ChangeBase(int number, StatType stat, int former, int value)
    {
      var pm = RomData.GetPokemon(number);
      if (pm.formData[0]._base.GetStat(stat) != former)
        return false;
      pm.formData[0]._base.SetStat(stat, value);
      return true;
    }
    static void Main(string[] args)
    {
      var rom = RomData.current = RomData.LoadFromXml<RomData>("..\\doc\\rom.xml");
      //GameString.Load("..\\res\\string", "zh", "en");
ChangeBase(025, StatType.Def, 30, 40);
ChangeBase(025, StatType.SpDef, 40, 50);
ChangeBase(012, StatType.SpAtk, 80, 90);
ChangeBase(015, StatType.Atk, 80, 90);
ChangeBase(018, StatType.Speed, 91, 101);
ChangeBase(026, StatType.Speed, 100, 110);
ChangeBase(031, StatType.Atk, 82, 92);
ChangeBase(034, StatType.Atk, 92, 102);
ChangeBase(036, StatType.SpAtk, 85, 95);
ChangeBase(040, StatType.SpAtk, 75, 85);
ChangeBase(045, StatType.SpAtk, 100, 110);
ChangeBase(062, StatType.Atk, 85, 95);
ChangeBase(065, StatType.SpDef, 85, 95);
ChangeBase(071, StatType.SpDef, 60, 70);
ChangeBase(076, StatType.Atk, 110, 120);
ChangeBase(181, StatType.Def, 75, 85);
ChangeBase(182, StatType.Def, 85, 95);
ChangeBase(184, StatType.SpAtk, 50, 60);
ChangeBase(189, StatType.SpDef, 85, 95);
ChangeBase(267, StatType.SpAtk, 90, 100);
ChangeBase(295, StatType.SpDef, 63, 73);
ChangeBase(398, StatType.SpDef, 50, 60);
ChangeBase(407, StatType.Def, 55, 65);
ChangeBase(508, StatType.Atk, 100, 110);
ChangeBase(521, StatType.Atk, 105, 115);
ChangeBase(526, StatType.SpDef, 70, 80);
ChangeBase(537, StatType.Atk, 85, 95);
ChangeBase(542, StatType.SpDef, 70, 80);
ChangeBase(545, StatType.Atk, 90, 100);
ChangeBase(553, StatType.Def, 70, 80);

      RomData.current.SaveXml("..\\doc\\rom.xml");
    }
  }
}
