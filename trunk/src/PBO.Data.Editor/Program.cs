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
      //using (var sw = new StreamWriter(Desktop + "abs.txt"))
      //{
      //  Action<int> PrintForm = (id) =>
      //    {
      //      var data = RomData.current.pokemons[id / 100 - 1].formData[id % 100];
      //      sw.WriteLine(data.Abilities[0] + " " + data.Abilities[1] + " " + data.Abilities[2]);
      //    };
      //  PrintForm(65000);
      //  PrintForm(65100);
      //  PrintForm(65200);
      //  PrintForm(65300);
      //  PrintForm(65400);
      //  PrintForm(65500);
      //  PrintForm(65600);
      //  PrintForm(65700);
      //  PrintForm(65800);
      //  PrintForm(65900);
      //  PrintForm(66000);
      //  PrintForm(66100);
      //  PrintForm(66200);
      //  PrintForm(66300);
      //  PrintForm(66400);
      //  PrintForm(66500);
      //  PrintForm(66600);
      //  PrintForm(66700);
      //  PrintForm(66800);
      //  PrintForm(66900);
      //  PrintForm(67000);
      //  PrintForm(67100);
      //  PrintForm(67200);
      //  PrintForm(67300);
      //  PrintForm(67400);
      //  PrintForm(67500);
      //  PrintForm(67600);
      //  PrintForm(67700);
      //  PrintForm(67800);
      //  PrintForm(67900);
      //  PrintForm(68000);
      //  PrintForm(68100);
      //  PrintForm(68200);
      //  PrintForm(68300);
      //  PrintForm(68400);
      //  PrintForm(68500);
      //  PrintForm(68600);
      //  PrintForm(68700);
      //  PrintForm(68800);
      //  PrintForm(68900);
      //  PrintForm(69000);
      //  PrintForm(69100);
      //  PrintForm(69200);
      //  PrintForm(69300);
      //  PrintForm(69400);
      //  PrintForm(69500);
      //  PrintForm(69600);
      //  PrintForm(69700);
      //  PrintForm(69800);
      //  PrintForm(69900);
      //  PrintForm(70000);
      //  PrintForm(70100);
      //  PrintForm(70200);
      //  PrintForm(70300);
      //  PrintForm(70400);
      //  PrintForm(70500);
      //  PrintForm(70600);
      //  PrintForm(70700);
      //  PrintForm(70800);
      //  PrintForm(70900);
      //  PrintForm(71000);
      //  PrintForm(71100);
      //  PrintForm(71200);
      //  PrintForm(71300);
      //  PrintForm(71400);
      //  PrintForm(71500);
      //  PrintForm(71600);
      //  PrintForm(71700);
      //  PrintForm(71800);
      //  PrintForm(71900);
      //  PrintForm(72000);
      //  PrintForm(72100);
      //  PrintForm(67801);
      //  PrintForm(68101);
      //  PrintForm(71001);
      //  PrintForm(71002);
      //  PrintForm(71003);
      //  PrintForm(71101);
      //  PrintForm(71102);
      //  PrintForm(71103);
      //  PrintForm(00301);
      //  PrintForm(00601);
      //  PrintForm(00602);
      //  PrintForm(00901);
      //  PrintForm(06501);
      //  PrintForm(09401);
      //  PrintForm(11501);
      //  PrintForm(12701);
      //  PrintForm(13001);
      //  PrintForm(14201);
      //  PrintForm(15001);
      //  PrintForm(15002);
      //  PrintForm(18101);
      //  PrintForm(21201);
      //  PrintForm(21401);
      //  PrintForm(22901);
      //  PrintForm(24801);
      //  PrintForm(25701);
      //  PrintForm(28201);
      //  PrintForm(30301);
      //  PrintForm(30601);
      //  PrintForm(30801);
      //  PrintForm(31001);
      //  PrintForm(35401);
      //  PrintForm(35901);
      //  PrintForm(44501);
      //  PrintForm(44801);
      //  PrintForm(46001);
      //}
      //var a = new int[189];
      //a[182] = 166;
      //a[186] = 167;
      //a[184] = 168;
      //a[179] = 169;
      //a[187] = 170;
      //a[185] = 171;
      //a[169] = 172;
      //a[170] = 173;
      //a[183] = 174;
      //a[173] = 175;
      //a[177] = 176;
      //a[178] = 177;
      //a[188] = 178;
      //a[172] = 179;
      //a[171] = 180;
      //a[168] = 181;
      //a[181] = 182;
      //a[176] = 183;
      //a[167] = 184;
      //a[174] = 185;
      //a[175] = 186;
      //a[180] = 187;
      //a[166] = 188;
      //foreach(var pm in rom.pokemons)
      //  foreach (var fd in pm.formData)
      //  {
      //    var fa = fd.Abilities[0];
      //    if (fa > 165) fd.Abilities[0] = a[fa];
      //    fa = fd.Abilities[1];
      //    if (fa > 165) fd.Abilities[1] = a[fa];
      //    fa = fd.Abilities[2];
      //    if (fa > 165) fd.Abilities[2] = a[fa];
      //  }
      var fm = rom.moves;
      rom.moves = new MoveType[616];
      for (int i = 0; i < 559; ++i) rom.moves[i] = fm[i];
      rom.moves[559] = fm[565]; rom.moves[559]._id = 560;
      rom.moves[560] = fm[564]; rom.moves[560]._id = 561;
      rom.moves[561] = fm[567]; rom.moves[561]._id = 562;
      rom.moves[562] = fm[570]; rom.moves[562]._id = 563;
      rom.moves[563] = fm[572]; rom.moves[563]._id = 564;
      rom.moves[564] = fm[571]; rom.moves[564]._id = 565;
      rom.moves[565] = fm[575]; rom.moves[565]._id = 566;
      rom.moves[566] = fm[576]; rom.moves[566]._id = 567;
      rom.moves[567] = fm[559]; rom.moves[567]._id = 568;
      rom.moves[568] = fm[588]; rom.moves[568]._id = 569;
      rom.moves[569] = fm[587]; rom.moves[569]._id = 570;
      rom.moves[570] = fm[583]; rom.moves[570]._id = 571;
      rom.moves[571] = fm[582]; rom.moves[571]._id = 572;
      rom.moves[572] = fm[591]; rom.moves[572]._id = 573;
      rom.moves[573] = fm[597]; rom.moves[573]._id = 574;
      rom.moves[574] = fm[592]; rom.moves[574]._id = 575;
      rom.moves[575] = fm[593]; rom.moves[575]._id = 576;
      rom.moves[576] = fm[600]; rom.moves[576]._id = 577;
      rom.moves[577] = fm[599]; rom.moves[577]._id = 578;
      rom.moves[578] = fm[602]; rom.moves[578]._id = 579;
      rom.moves[579] = fm[580]; rom.moves[579]._id = 580;
      rom.moves[580] = fm[604]; rom.moves[580]._id = 581;
      rom.moves[581] = fm[586]; rom.moves[581]._id = 582;
      rom.moves[582] = fm[596]; rom.moves[582]._id = 583;
      rom.moves[583] = fm[606]; rom.moves[583]._id = 584;
      rom.moves[584] = fm[605]; rom.moves[584]._id = 585;
      rom.moves[585] = fm[562]; rom.moves[585]._id = 586;
      rom.moves[586] = fm[601]; rom.moves[586]._id = 587;
      rom.moves[587] = fm[577]; rom.moves[587]._id = 588;
      rom.moves[588] = fm[561]; rom.moves[588]._id = 589;
      rom.moves[589] = fm[560]; rom.moves[589]._id = 590;
      rom.moves[590] = fm[607]; rom.moves[590]._id = 591;
      rom.moves[593] = fm[579]; rom.moves[593]._id = 594;
      rom.moves[594] = fm[578]; rom.moves[594]._id = 595;
      rom.moves[595] = fm[581]; rom.moves[595]._id = 596;
      rom.moves[596] = fm[594]; rom.moves[596]._id = 597;
      rom.moves[597] = fm[584]; rom.moves[597]._id = 598;
      rom.moves[598] = fm[568]; rom.moves[598]._id = 599;
      rom.moves[599] = fm[573]; rom.moves[599]._id = 600;
      rom.moves[600] = fm[595]; rom.moves[600]._id = 601;
      rom.moves[601] = fm[585]; rom.moves[601]._id = 602;
      rom.moves[603] = fm[590]; rom.moves[603]._id = 604;
      rom.moves[604] = fm[603]; rom.moves[604]._id = 605;
      rom.moves[607] = fm[598]; rom.moves[607]._id = 608;
      rom.moves[608] = fm[589]; rom.moves[608]._id = 609;
      rom.moves[610] = fm[574]; rom.moves[610]._id = 611;
      rom.moves[611] = fm[563]; rom.moves[611]._id = 612;
      rom.moves[612] = fm[566]; rom.moves[612]._id = 613;
      rom.moves[615] = fm[569]; rom.moves[615]._id = 616;
      for (int i = 592; i < rom.moves.Length; ++i)
        if (rom.moves[i] == null)
        {
          rom.moves[i] = new MoveType(i + 1);
          rom.moves[i]._class = MoveInnerClass.Special;
          rom.moves[i]._category = MoveCategory.Status;
          rom.moves[i]._pp = 10;
          rom.moves[i]._type = BattleType.Normal;
        }
      RomData.current.SaveXml("..\\doc\\rom.xml");
    }
  }
}
