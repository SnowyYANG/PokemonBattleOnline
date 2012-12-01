using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using LightStudio.Tactic.DataModels;
using LightStudio.Tactic;

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
    static void Main(string[] args)
    {
      RomData r = RomData.Load("Data\\rom.dat");
      using(StreamWriter sw = new StreamWriter(Desktop + "moves.csv", false, Encoding.Default))
        foreach (var m in r.Moves)
        {
          sw.Write(m.Id);
          sw.Write(',');
          sw.Write(m.EnglishName);
          sw.Write(',');
          sw.Write(m.Class);
          sw.Write(',');
          sw.Write(m.Category);
          sw.Write(',');
          sw.Write(m.Power);
          sw.Write(',');
          sw.Write(m.Accuracy);
          sw.Write(',');
          sw.Write(m.PP);
          sw.Write(',');
          sw.Write(m.Priority);
          sw.Write(',');
          sw.Write(m.MaxTimes);
          sw.Write(m.MinTimes);
          sw.Write(',');
          sw.Write(m.CtLv);
          sw.Write(',');
          sw.Write(m.FlinchProbability);
          sw.Write(',');
          sw.Write(m.HurtPercentage);
          sw.Write(',');
          sw.Write(m.MaxHpPercentage);
          sw.Write(',');
          sw.Write(m.Range);
          sw.Write(',');
          if (m.Flags.AvailableEvenFrozen) sw.Write("解冻");
          sw.Write(',');
          if (m.Flags.IgnoreSubstitute) sw.Write("替身");
          sw.Write(',');
          if (m.Flags.IsFist) sw.Write("拳击");
          sw.Write(',');
          if (m.Flags.IsHeal) sw.Write("回复");
          sw.Write(',');
          if (m.Flags.IsRemote) sw.Write("远程");
          sw.Write(',');
          if (m.Flags.IsSound) sw.Write("声音");
          sw.Write(',');
          if (m.Flags.MagicCoat) sw.Write("魔反");
          sw.Write(',');
          if (m.Flags.Mirrorable) sw.Write("鹦鹉");
          sw.Write(',');
          if (m.Flags.NeedTouch) sw.Write("接触");
          sw.Write(',');
          if (m.Flags.PrepareOneTurn) sw.Write("蓄力");
          sw.Write(',');
          if (m.Flags.Protectable) sw.Write("保护");
          sw.Write(',');
          if (m.Flags.Snatchable) sw.Write("抢夺");
          sw.Write(',');
          if (m.Flags.StiffOneTurn) sw.Write("僵直");
          sw.Write(',');
          if (m.Flags.UnavailableWithGravity) sw.Write("重力");
          sw.WriteLine();
        }
    }
  }
}
