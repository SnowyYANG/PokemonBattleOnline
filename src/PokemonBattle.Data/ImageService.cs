using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO;
using System.Diagnostics.Contracts;
using LightStudio.PokemonBattle.Data;

namespace LightStudio.PokemonBattle.Data
{
  public static class ImageService
  {
    private static readonly Dictionary<int, ImageSource> icons;

    static ImageService()
    {
      icons = new Dictionary<int,ImageSource>();
    }

    private static ImageSource GetImage(string path, string id)
    {
      string absolutePath = string.Format("Data\\image\\{0}\\{1}.png", path, id);
      try
      {
        return new BitmapImage(new Uri(absolutePath, UriKind.Absolute));
      }
      catch
      {
        return null;
      }
    }
    private static ImageSource GetImage(string path, int number, int forme)
    {
      return GetImage(path, (number * 100 + forme).ToString("00000"));
    }
    public static ImageSource GetPokemonIcon(int number, int forme)
    {
      int id = number * 100 + forme;
      ImageSource r;
      if (!icons.TryGetValue(id, out r))
      {
        r = GetImage("icon", number, forme);
        icons[id] = r;
      }
      return r;
    }
    public static ImageSource GetPokemonMaleFront(int number, int forme)
    {
      return GetImage("front", number, forme);
    }
    public static ImageSource GetPokemonFemaleFront(int number, int forme)
    {
      ImageSource image = GetImage("front\\female", number, forme);
      if (image == null) image = GetPokemonMaleFront(number, forme);
      return image;
    }
    public static ImageSource GetPokemonMaleBack(int number, int forme)
    {
      return GetImage("back", number, forme);
    }
    public static ImageSource GetPokemonFemaleBack(int number, int forme)
    {
      ImageSource image = GetImage("back\\female", number, forme);
      if (image == null) image = GetPokemonMaleBack(number, forme);
      return image;
    }
    public static ImageSource GetSpFront(int number, int forme)
    {
      return GetImage("front\\sp", number, forme);
    }
    public static ImageSource GetSpBack(string id)
    {
      return GetImage("back\\sp", id);
    }
  }
}
