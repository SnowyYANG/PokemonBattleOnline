using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Media.Imaging;
using System.IO.Packaging;

namespace PokemonBattleOnline.Data
{
  internal class ImageService : IDisposable
  {
    private readonly Package Pack;
    private BitmapImage[] icons;

    public ImageService(string pack)
    {
      icons = new BitmapImage[GameDataService.Pokemons.Count() + 1];
      Pack = ZipPackage.Open(new FileStream(pack, FileMode.Open, FileAccess.Read, FileShare.Read));
    }

    private BitmapImage GetImage(string path, string id)
    {
      try
      {
        var image = new BitmapImage();
        image.BeginInit();
        image.StreamSource = Pack.GetPart(new Uri(string.Format("/{0}/{1}.png", path, id), UriKind.Relative)).GetStream(FileMode.Open, FileAccess.Read);
        image.EndInit();
        return image;
      }
      catch
      {
        return null;
      }
    }
    private BitmapImage GetImage(string path, int number, int form)
    {
      return GetImage(path, (number * 100 + form).ToString("00000"));
    }
    private BitmapImage GetImage(string path, PokemonForm form)
    {
      return GetImage(path, form.Type.Number, form.Index);
    }
    private BitmapImage GetFemaleImage(string path, PokemonForm form)
    {
      var i = GetImage(path + "/female", form);
      if (i == null) i = GetImage(path, form);
      return i;
    }
    private BitmapImage GetPokemonImage(string category, PokemonForm form, PokemonGender gender, bool shiny)
    {
      var path = shiny ? "shiny/" : "normal/" + category;
      return gender == PokemonGender.Female ? GetFemaleImage(path, form) : GetImage(path, form);
    }
    public BitmapImage GetPokemonIcon(PokemonForm form, PokemonGender gender)
    {
      int n = form.Type.Number, f = form.Index;
      BitmapImage r;
      if (gender == PokemonGender.Female && (n == 521 || n == 592 || n == 593)) r = GetImage("icon/female", n, f);
      else if (f == 0 || n == 493 || n == 649) //arceus and genesect
      {
        if (icons[n] == null) icons[n] = GetImage("icon", n, 0);
        r = icons[n];
      }
      else r = GetImage("icon", n, f);
      return r;
    }
    public BitmapImage GetPokemonFront(PokemonForm form, PokemonGender gender, bool shiny)
    {
      return GetPokemonImage("front", form, gender, shiny);
    }
    public BitmapImage GetPokemonBack(PokemonForm form, PokemonGender gender, bool shiny)
    {
      return GetPokemonImage("back", form, gender, shiny);
    }
    public BitmapImage GetSpFront(string id)
    {
      return GetImage("sp/front", id);
    }
    public BitmapImage GetSpBack(string id)
    {
      return GetImage("sp/back", id);
    }

    public void Dispose()
    {
      ((IDisposable)Pack).Dispose();
    }
  }
}
