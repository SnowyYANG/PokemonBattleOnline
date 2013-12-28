using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PokemonBattleOnline.Game
{
  public static class LearnList
  {
    public static readonly GenLearnList[] Index;
    public static readonly SPLearnList SP;

    static LearnList()
    {
      Index = new GenLearnList[PBOMarks.GEN - 2];
      SP = new SPLearnList();
    }

    public static void Load(ZipData zip, string path)
    {
      using (var index = new StreamReader(zip.GetStream(path + "/index.txt")))
      {
        var gll = new GenLearnList(3);
        var lvs = new Dictionary<string, LvLearnList>();
        var tmhmts = new Dictionary<string, TMHMTutorLearnList>();
        var eggs = new Dictionary<string, EggLearnList>();
        for (string line = index.ReadLine(); !string.IsNullOrWhiteSpace(line); line = index.ReadLine())
        {
          var s = Split(line);
          var gen = Convert.ToInt32(s[0]);
          if (gen != gll.Gen)
          {
            Index[gll.Gen - 3] = gll;
            gll = new GenLearnList(gen);
          }
          LvLearnList lv = null;
          EggLearnList egg = null;
          TMHMTutorLearnList tm = null, hm = null, tutor = null;
          for (int i = 2; i < s.Length; ++i)
          {
            var table = s[i];
            switch (table[1])
            {
              case 'e':
                {
                  lv = lvs.ValueOrDefault(table);
                  if (lv == null)
                  {
                    lv = new LvLearnList();
                    LoadLevel(GetStream(zip, path, table), lv);
                    lvs.Add(table, lv);
                  }
                }
                break;
              case 'g':
                {
                  egg = eggs.ValueOrDefault(table);
                  if (egg == null)
                  {
                    egg = new EggLearnList();
                    LoadEgg(GetStream(zip, path, table), egg);
                    eggs.Add(table, egg);
                  }
                }
                break;
              case 'M':
                {
                  var needHM = gen == PBOMarks.GEN;
                  var g = table.Substring(5);
                  var tmk = "TM_" + g;
                  tm = tmhmts.ValueOrDefault(tmk);
                  if (tm == null)
                  {
                    tm = new TMHMTutorLearnList();
                    if (needHM) hm = new TMHMTutorLearnList();
                    LoadTMHM(GetStream(zip, path, table), tm, hm);
                    tmhmts.Add(tmk, tm);
                    if (needHM) tmhmts.Add("HM_" + g, hm);
                  }
                  else if (needHM) hm = tmhmts["HM_" + g];
                }
                break;
              case 'u':
                {
                  tutor = tmhmts.ValueOrDefault(table);
                  if (tutor == null)
                  {
                    tutor = new TMHMTutorLearnList();
                    LoadTutor(GetStream(zip, path, table), tutor);
                    tmhmts.Add(table, tutor);
                  }
                }
                break;
            }
          }//for (int i = 2;
          gll.Add(s[1], lv, egg, tm, tutor, hm);
        }
        Index[gll.Gen - 3] = gll;
      }
      using (var sr = new StreamReader(zip.GetStream(path + "/SP.txt")))
      {
        var moves = new List<int>();
        for (string line = sr.ReadLine(); !string.IsNullOrWhiteSpace(line); line = sr.ReadLine())
        {
          var s = Split(line);
          var number = s[0].ToInt();
          var form = s[1].ToInt();
          moves.Clear();
          for (int i = 2; i < s.Length; ++i) moves.Add(s[i].ToInt());
          SP.Set(number, form, moves.ToArray());
        }
      }
    }
    private static readonly char[] SPLIT_CHARS = new char[] { '.', '\t', ',' };
    private static string[] Split(string str)
    {
      return str.Split(SPLIT_CHARS, StringSplitOptions.RemoveEmptyEntries);
    }
    private static Stream GetStream(ZipData zip, string path, string table)
    {
      return zip.GetStream(path + "/" + table + ".txt");
    }
    private static void LoadLevel(Stream stream, LvLearnList lv)
    {
      var mls = new List<KeyValuePair<int, int>>();
      using (var sr = new StreamReader(stream))
        for (string line = sr.ReadLine(); !string.IsNullOrWhiteSpace(line); line = sr.ReadLine())
        {
          //[0].[1] [2],[3]...
          var s = Split(line);
          for (int i = 2; i < s.Length - 1; ++i)
          {
            //move:lv
            var ml = s[i].Split(':');
            mls.Add(new KeyValuePair<int, int>(ml[0].ToInt(), ml[1].ToInt()));
          }
          if (mls.Any())
          {
            lv.Set(Convert.ToInt32(s[0]), Convert.ToInt32(s[1]), mls.ToArray());
            mls.Clear();
          }
        }
    }
    private static void LoadEgg(Stream stream, EggLearnList egg)
    {
      var moves = new List<int>();
      using (var sr = new StreamReader(stream))
        for (string line = sr.ReadLine(); !string.IsNullOrWhiteSpace(line); line = sr.ReadLine())
        {
          //[0].[1] [2],[3]...
          var s = Split(line);
          for (int i = 2; i < s.Length - 1; ++i) moves.Add(Convert.ToInt32(s[i]));
          if (moves.Any())
          {
            egg.Set(Convert.ToInt32(s[0]), moves.ToArray());
            moves.Clear();
          }
        }
    }
    private static void LoadTMHM(Stream stream, TMHMTutorLearnList tm, TMHMTutorLearnList hm)
    {
      var tmmoves = new List<int>();
      var hmmoves = hm == null ? null : new List<int>();
      using (var sr = new StreamReader(stream))
        for (string line = sr.ReadLine(); !string.IsNullOrWhiteSpace(line); line = sr.ReadLine())
        {
          //[0].[1] [2],[3]...
          var s = Split(line);
          for (int i = 2; i < s.Length - 1; ++i)
          {
            //move:TM## move:HM## TM00=TM100
            var mm = s[i];
            var move = Convert.ToInt32(mm.Substring(0, mm.Length - 5));
            if (mm[mm.Length - 4] == 'T') tmmoves.Add(move);
            else if (hmmoves == null) break;
            else hmmoves.Add(move);
          }
          var number = Convert.ToInt32(s[0]);
          var form = Convert.ToInt32(s[1]);
          if (tmmoves.Any())
          {
            tm.Set(number, form, tmmoves.ToArray());
            tmmoves.Clear();
          }
          if (hmmoves != null && hmmoves.Any())
          {
            hm.Set(number, form, hmmoves.ToArray());
            hmmoves.Clear();
          }
        }
    }
    private static void LoadTutor(Stream stream, TMHMTutorLearnList tutor)
    {
      var moves = new List<int>();
      using (var sr = new StreamReader(stream))
        for (string line = sr.ReadLine(); !string.IsNullOrWhiteSpace(line); line = sr.ReadLine())
        {
          //[0].[1] [2],[3]...
          var s = Split(line);
          for (int i = 2; i < s.Length - 1; ++i) moves.Add(s[i].ToInt());
          if (moves.Any())
          {
            tutor.Set(s[0].ToInt(), s[1].ToInt(), moves.ToArray());
            moves.Clear();
          }
        }
    }
  }
}
