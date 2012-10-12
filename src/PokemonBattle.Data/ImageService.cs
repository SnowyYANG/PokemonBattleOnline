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
    private static ImageSource[] icons;

    static ImageService()
    {
      icons = new ImageSource[GameDataService.Pokemons.Count()];
    }

    private static ImageSource GetImage(string path, string id)
    {
      string absolutePath = Path.GetFullPath(string.Format("Data\\image\\{0}\\{1}.png", path, id));
      try
      {
        return new BitmapImage(new Uri(absolutePath, UriKind.Absolute));
      }
      catch
      {
        return null;
      }
    }
    private static ImageSource GetImage(string path, int number, int form)
    {
      return GetImage(path, (number * 100 + form).ToString("00000"));
    }
    private static ImageSource GetImage(string path, PokemonForm form)
    {
      return GetImage(path, form.Type.Number, form.Index);
    }
    private static ImageSource GetFemaleImage(string path, PokemonForm form)
    {
      var i = GetImage(path + "\\female", form);
      if (i == null) GetImage(path, form);
      return i;
    }
    public static ImageSource GetPokemonIcon(PokemonForm form, PokemonGender gender)
    {
      int n = form.Type.Number, f = form.Index;
      ImageSource r;
      if (gender == PokemonGender.Female && (n == 521 || n == 592 || n == 593)) r = GetImage("icon\\female", n, f);
      else if (f == 0 || n == 493 || n == 649) //arceus and genesect
      {
        if (icons[n] == null) icons[n] = GetImage("icon", n, 0);
        r = icons[n];
      }
      else r = GetImage("icon", n, f);
      return r;
    }
    public static ImageSource GetPokemonFront(PokemonForm form, PokemonGender gender)
    {
      return gender == PokemonGender.Female ? GetFemaleImage("front", form) : GetImage("front", form);
    }
    public static ImageSource GetPokemonBack(PokemonForm form, PokemonGender gender)
    {
      return gender == PokemonGender.Female ? GetFemaleImage("back", form) : GetImage("back", form);
    }
    public static ImageSource GetSpFront(string id)
    {
      return GetImage("front\\sp", id);
    }
    public static ImageSource GetSpBack(string id)
    {
      return GetImage("back\\sp", id);
    }
  }
}
