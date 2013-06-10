﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace LightStudio.PokemonBattle.PBO.Converters
{
  public class IdAvatar : Converter<int>
  {
    public static readonly IdAvatar C = new IdAvatar();
    private static readonly Dictionary<int, BitmapImage> _avatars;

    static IdAvatar()
    {
      _avatars = new Dictionary<int, BitmapImage>(214);
      for (int i = 651; i <= 867; ++i)
      {
        if (i == 790 || i == 856 || i == 857 || i == 858) continue;
        var av = Data.GameDataService.GetAvatar(i);
        _avatars.Add(i, av);
      }
    }

    public static IEnumerable<int> Avatars
    { get { return _avatars.Keys; } }

    public static BitmapImage GetAvatar(int value)
    {
      return _avatars.ValueOrDefault(value);
    }

    protected override object Convert(int value)
    {
      return GetAvatar(value);
    }
  }
}
